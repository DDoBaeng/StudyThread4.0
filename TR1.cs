using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StudyThread4._0
{
    public class TR1
    {
        ///
        /// 정석적인 쓰레드 사용 비동기 동작방식
        ///


        /// <summary>
        /// 최대 TASK와 동시에 진행 할 Workers를 설정하여 비동기 쓰레드 동작을 진행함
        /// TOTAL_TASKS : 최대 일 숫자
        /// MAX_CONCURRENT_WORKERS : 동시에 처리 할 최대 작업 개수
        /// Semaphore : 동시 실행 쓰레드 수를 설정할 수 있는 클래스 (초기항목수, 최대항목수)
        /// ManualResetEvent : 모든 작업이 완료될 때 까지 메인 쓰레드를 대기시키기 위한 동기화 객체
        /// ThreadPool.QueueUserWorkItem("할일", State) : 작업 풀에 작업을 큐잉함 
        /// 스택(Stack) : LIFO
        /// 큐(Queue) : FIFO
        /// ManualResetEvent.WaitOne(); : 모든 작업 완료 신호를 기다린다.
        /// Semaphore.WaitOne(): 동시 실행 제한을 지킵니다. (최대 MAX_CONCURRENT_WORKERS 개만 진입)
        /// Semaphore.Release(): 동시 실행 슬롯을 반환합니다.
        /// ManualResetEvent.Set(): 마지막 작업 완료 시 메인 스레드 해제
        /// Interlocked.Decrement: 남은 작업 수를 스레드 안전하게 1 감소 (비동기방식의 싱크를 맞추기 위함)
        /// </summary>


        private const int TOTAL_TASKS = 50;
        private const int MAX_CONCURRENT_WORKERS = 5; // 🚦 Semaphore 제한: 최대 3개 동시 실행

        // 남은 작업 수를 스레드 안전하게 추적할 변수
        private int s_remainingTasks;

        // 모든 작업이 완료될 때까지 메인 스레드를 대기시키기 위한 동기화 객체
        private ManualResetEvent s_allDoneEvent = new ManualResetEvent(false);

        // 동시 실행 스레드 수를 제한하는 세마포어
        // 초기 카운트 3, 최대 카운트 3 (동시에 3개만 허용)
        // 동시 실행 스레드 수 제한
        private Semaphore s_concurrentLimit = new Semaphore(
            initialCount: MAX_CONCURRENT_WORKERS,
            maximumCount: MAX_CONCURRENT_WORKERS
        );

        public void Main()
        {
            Console.WriteLine($"[Main] 총 {TOTAL_TASKS}개의 작업을 시작합니다. 동시 실행 제한: {MAX_CONCURRENT_WORKERS}개");

            // 1. 남은 작업 수 초기화
            s_remainingTasks = TOTAL_TASKS;

            // 2. 10개의 작업을 ThreadPool에 등록
            for (int i = 0; i < TOTAL_TASKS; i++)
            {
                ThreadPool.QueueUserWorkItem(DoWork, i + 1);
            }

            // 3. 모든 작업 완료 대기
            Console.WriteLine("[Main] 모든 작업 완료 신호를 기다립니다...");
            s_allDoneEvent.WaitOne();

            Console.WriteLine("[Main] 모든 작업이 완료되었습니다. 프로그램 종료.");

            //// 1. 데이터 준비: 1부터 1000까지의 정수 리스트
            //List<int> numbers = Enumerable.Range(1, 1000).ToList();

            //// 2. 최종 합계를 저장할 변수 (스레드 안전하게 접근해야 함)
            //long totalSum = 0;

            //var option = new ParallelOptions
            //{
            //    MaxDegreeOfParallelism = 1
            //};

            //// 3. Parallel.ForEach를 사용하여 병렬 처리 시작
            //// Parallel.ForEach는 내부적으로 ThreadPool을 사용하여 작업을 분산합니다.
            //Parallel.ForEach(numbers, number =>
            //{
            //    // 각 숫자의 제곱을 계산하는 작업 (CPU 집약적 작업 가정)
            //    long square = (long)number * number;

            //    // ⚠️ 주의: 여러 스레드가 동시에 totalSum에 접근합니다.
            //    // 일반적인 'totalSum += square;'는 데이터 손실(Race Condition)을 일으킵니다.

            //    // 4. Interlocked를 사용하여 스레드 안전하게 합계에 추가 (Lock-Free 방식)
            //    // Interlocked.Add는 원자적(Atomic)으로 값을 더해 스레드 안전성을 보장합니다.
            //    System.Threading.Interlocked.Add(ref totalSum, square);

            //    // 어떤 스레드가 이 작업을 실행했는지 확인 (결과는 예측 불가능)
            //    Console.WriteLine($"Number: {number,-4} | Square: {square,-7} | Thread ID: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            //});

            //Console.WriteLine("\n------------------------------------------------");
            //Console.WriteLine($"모든 병렬 작업 완료.");
            //Console.WriteLine($"총 합계 (스레드 안전): {totalSum}");
            //// 1부터 1000까지 제곱의 합 공식: 333833500

            Console.ReadLine();
        }

        private void DoWork(object state)
        {
            int taskId = (int)state;
            int threadId = Thread.CurrentThread.ManagedThreadId;

            Console.WriteLine($"[TID:{threadId}] Task {taskId}: 세마포어 진입을 대기합니다.");

            // 4. Semaphore.WaitOne(): 동시 실행 제한을 지킵니다. (최대 3개만 진입)
            s_concurrentLimit.WaitOne();

            try
            {
                // 💡 세마포어에 의해 동시 실행이 제한된 핵심 작업 영역 (Critical Section)
                Console.WriteLine($"[TID:{threadId}] Task {taskId} >>> 핵심 작업 시작! ({DateTime.Now.ToLongTimeString()})");

                // 작업 수행 시간 가정
                Thread.Sleep(1500); // 1.5초 동안 작업

                Console.WriteLine($"[TID:{threadId}] Task {taskId} <<< 핵심 작업 완료.");
                Console.WriteLine($"[TASK:{s_remainingTasks - 1}] *** 남은 TASKS.");
            }
            finally
            {
                // 5. Semaphore.Release(): 동시 실행 슬롯을 반환합니다.
                s_concurrentLimit.Release();

                // 6. Interlocked.Decrement: 남은 작업 수를 스레드 안전하게 1 감소
                if (Interlocked.Decrement(ref s_remainingTasks) == 0)
                {
                    // 7. ManualResetEvent.Set(): 마지막 작업 완료 시 메인 스레드 해제
                    s_allDoneEvent.Set();
                    Console.WriteLine("[All Done] ManualResetEvent.Set() 호출.");
                }
            }
        }
    }
}
