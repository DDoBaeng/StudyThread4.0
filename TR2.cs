using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace StudyThread4._0
{
    public class TR2
    {
        /// <summary>
        /// ENTER 키를 입력받아 작업을 쓰레드로 돌리는 동작 (동적 처리 + 비동기)
        /// </summary>

        private const int MAX_CONCURRENT_WORKERS = 3;

        // 현재 활성화된(아직 완료되지 않은) 작업의 수
        private int s_activeTaskCount = 0;

        // 동시 실행 스레드 수 제한
        private Semaphore s_concurrentLimit = new Semaphore(
            initialCount: MAX_CONCURRENT_WORKERS,
            maximumCount: MAX_CONCURRENT_WORKERS
        );

        // 전체 작업을 취소하기 위한 토큰 소스
        private static CancellationTokenSource s_cts = new CancellationTokenSource();

        // 고유한 작업 ID 카운터
        private static int s_taskIdCounter = 0;

        public void Main()
        {

            while(true)
            {
                // true는 키를 화면에 표시하지 않음
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if(keyInfo.Key == ConsoleKey.Enter)
                {
                    // Interlocked.Increment를 사용하여 고유 ID 확보 및 활성 작업 수 증가
                    int newTaskId = Interlocked.Increment(ref s_taskIdCounter);
                    Interlocked.Increment(ref s_activeTaskCount);

                    ThreadPool.QueueUserWorkItem(DoWork, newTaskId);
                    Console.WriteLine($"\n[MAIN] Task {newTaskId}를 큐에 추가했습니다. (현재 활성: {s_activeTaskCount}개)");
                }
            }
        }

        private void DoWork(object state)
        {
            int taskId = (int)state;
            int threadId = Thread.CurrentThread.ManagedThreadId;

            // 1. 취소되었는지 확인
            if (s_cts.Token.IsCancellationRequested) return;

            Console.WriteLine($"[TID:{threadId}] Task {taskId}: 세마포어 진입 대기...");
            s_concurrentLimit.WaitOne(); // 동시 실행 제한 적용

            try
            {
                // 2. 핵심 작업 시작
                if (s_cts.Token.IsCancellationRequested) return; // 다시 확인

                Console.WriteLine($"[TID:{threadId}] Task {taskId} >>> 핵심 작업 시작! ");

                // 작업 수행 시간 가정
                Random rand = new Random(taskId);
                int randValue = rand.Next(1000, 3000);


                FormWork f = new FormWork();
                f.Show();

                f.BeginInvoke(new Action(() =>
                {
                    f.taskID = taskId;
                    f.closeTime = randValue;
                    f.BackColor = GetRandomRGBColor();
                }));
                

                //Thread.Sleep(randValue); // 1초에서 3초 사이 대기
                //f.Close();

                Console.WriteLine($"\n[TID:{threadId}] Task {taskId} <<< 핵심 작업 완료.");
            }
            finally
            {
                // 3. 작업 완료 후 동시 실행 슬롯 반환
                s_concurrentLimit.Release();

                // 4. 활성 작업 수 감소
                Interlocked.Decrement(ref s_activeTaskCount);
                Console.WriteLine($"[TID:{threadId}] Task {taskId} 종료. (남은 활성: {s_activeTaskCount}개)");
            }
        }

        private Color GetRandomRGBColor()
        {
            Random random = new Random();

            // Generate random values for Red, Green, and Blue components (0-255)
            int r = random.Next(256); // Next(maxValue) returns a value less than maxValue
            int g = random.Next(256);
            int b = random.Next(256);

            // Create a Color object from the RGB values
            return Color.FromArgb(r, g, b);
        }
    }
}
