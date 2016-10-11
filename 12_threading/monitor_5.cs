using System;
using System.Threading;
using System.Collections;

public class CrudeThreadPool
{
    static readonly int MAX_WORK_THREADS = 4;
    static readonly int WAIT_TIMEOUT = 2000;

    public delegate void WorkDelegate();

    public CrudeThreadPool() {
        stop = 0;
        workLock = new Object();
        workQueue = new Queue();
        threads = new Thread[ MAX_WORK_THREADS ];

        for( int i = 0; i < MAX_WORK_THREADS; ++i ) {
            threads[i] =
                new Thread( new ThreadStart(this.ThreadFunc) );
            threads[i].Start();
        }
    }

    private void ThreadFunc() {
        lock( workLock ) {
            int shouldStop = 0;
            do {
                shouldStop = Interlocked.Exchange( ref stop,
                                                   stop );
                if( shouldStop == 0 ) {
                    WorkDelegate workItem = null;
                    if( Monitor.Wait(workLock, WAIT_TIMEOUT) ) {
                        // Process the item on the front of the
                        // queue
                        lock( workQueue ) {
                            workItem =
                                (WorkDelegate) workQueue.Dequeue();
                        }
                        workItem();
                    }
                }
            } while( shouldStop == 0 );
        }
    }

    public void SubmitWorkItem( WorkDelegate item ) {
        lock( workLock ) {
            lock( workQueue ) {
                workQueue.Enqueue( item );
            }

            Monitor.Pulse( workLock );
        }
    }

    public void Shutdown() {
        Interlocked.Exchange( ref stop, 1 );
    }

    private Queue    workQueue;
    private Object   workLock;
    private Thread[] threads;
    private int      stop;
}

public class EntryPoint
{
    static void WorkFunction() {
        Console.WriteLine( "WorkFunction() called on Thread {0}",
                           Thread.CurrentThread.GetHashCode() );
    }

    static void Main() {
        CrudeThreadPool pool = new CrudeThreadPool();
        for( int i = 0; i < 10; ++i ) {
            pool.SubmitWorkItem(
              new CrudeThreadPool.WorkDelegate(
                                     EntryPoint.WorkFunction) );
        }

        pool.Shutdown();
    }
}
