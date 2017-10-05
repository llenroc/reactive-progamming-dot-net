﻿using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Reactive.PlatformServices;
using System.Linq;

namespace Lab_02
{
    public class Program {
        static void Main (string[] args) {
            
            var threadId = Thread.CurrentThread.ManagedThreadId.ToString ();
            Console.WriteLine ($"Program is runing on thread with Id:{threadId}");
            
            Demo();

            Console.WriteLine("ByeBye");
        }

        static void Demo(){
            var isContinue = true;
            
            do{
                Console.WriteLine("======Press demo key to represent...=====");
                Console.WriteLine("(a) Demo MyObserver");
                Console.WriteLine("(b) Demo MyObserver with Lambda");
                Console.WriteLine("(c) Demo MyObserver with Lambda and NewThread");
                Console.WriteLine("(d) Demo MyObservable");
                Console.WriteLine("(e) Demo Linq With Subscribling");
                Console.WriteLine("(q) Exit Menu");

                var keyInfo = Console.ReadKey();
                Console.WriteLine();

                switch(keyInfo.Key){
                    case ConsoleKey.Q:
                        isContinue = false;
                        break;
                    case ConsoleKey.A:
                        Demo_MyObserver();
                        break;
                    case  ConsoleKey.B:
                        Demo_MyObserverLambda();
                        break;
                    case  ConsoleKey.C:
                        Demo_MyObserverLambdaWithNewThread();
                        break;
                    case  ConsoleKey.D:
                        Demo_MyObservable();
                        break;
                    case  ConsoleKey.F:
                        Demo_LinqWithSubscribling();
                        break;
                    default:
                        Console.WriteLine("Unknow, please re-try!!");
                        break;
                } 

            }while(isContinue); 
        }

        static void Demo_LinqWithSubscribling(){
            var result = 
                from i in Observable.Range(1, 100) 
                from j in Observable.Range(1, 100) 
                from k in Observable.Range(1, 100) 
                where k * k == i * i + j * j 
                select new { a = i, b = j, c = k };   
        
            IDisposable subscription = result.Subscribe( 
            x => Console.WriteLine("OnNext: {0}", x),  
            ex => Console.WriteLine("OnError: {0}", ex.Message), 
            () => Console.WriteLine("OnCompleted")); 
        }

        static void Demo_MyObservable () {
            var observable = new MyObservable(5,8);
            
            var observer = new MyObserver();

            var subscription = observable.Subscribe(observer);

            Console.WriteLine("Press any key to dispose the subscription...");
            Console.ReadKey();
            Console.WriteLine();
            subscription.Dispose();
        }
        
        static void Demo_MyObserverLambda () {
            var observable = Observable.Range(5, 8);
            
            var subscription = observable.Subscribe(
                (input) =>{Console.WriteLine($"Id:{Thread.CurrentThread.ManagedThreadId.ToString()}:{input}");},
                (err) => {Console.WriteLine(err.Message);},
                () => {Console.WriteLine("completed");}
            );

            Console.WriteLine("Press any key to dispose the subscription...");
            Console.ReadKey();
            Console.WriteLine();
            subscription.Dispose();
        }


        static void Demo_MyObserverLambdaWithNewThread () {
            
            var source = Observable.Range(5, 8)
                .SelectMany(i=>Observable.Start(()=>i, NewThreadScheduler.Default))
                //.ObserveOn(NewThreadScheduler.Default)
                .Subscribe(
                    (input) =>{Console.WriteLine($"Id:{Thread.CurrentThread.ManagedThreadId.ToString()}:{input}");},
                    (err) => {Console.WriteLine(err.Message);},
                    () => {Console.WriteLine("completed");});

            Console.WriteLine("Press any key to dispose the subscription...");
            Console.ReadKey();
            Console.WriteLine();
            source.Dispose();
        }

        static void Demo_MyObserver () {
            var observable = Observable.Range (5, 8);

            var observer = new MyObserver();

            var subscription = observable.Subscribe(observer);

            Console.WriteLine("Press any key to dispose the subscription...");
            Console.ReadKey ();
            Console.WriteLine();
            subscription.Dispose ();
        }
    }
}