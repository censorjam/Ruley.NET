using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace Ruley.Core
{
    public class ObservableFileWatcher
    {
        private readonly FileSystemWatcher _watcher;
        public IObservable<string> Created { get; private set; }
        public IObservable<string> Changed { get; private set; }
        public IObservable<string> Deleted { get; private set; }

        public ObservableFileWatcher(FileSystemWatcher watcher)
        {
            _watcher = watcher;
            _watcher.EnableRaisingEvents = true;
            Created = Observable.FromEvent<FileSystemEventHandler, FileSystemEventArgs>(handler =>
            {
                FileSystemEventHandler fsHandler = (sender, e) =>
                {
                    handler(e);
                };

                return fsHandler;
            },
                fsHandler => _watcher.Created += fsHandler,
                fsHandler => _watcher.Created -= fsHandler)
                .Select(x => x.FullPath);

            Changed = Observable.FromEvent<FileSystemEventHandler, FileSystemEventArgs>(handler =>
            {
                FileSystemEventHandler fsHandler = (sender, e) =>
                {
                    handler(e);
                };

                return fsHandler;
            },
                fsHandler => _watcher.Changed += fsHandler,
                fsHandler => _watcher.Changed -= fsHandler)
                .BufferWithInactivity(TimeSpan.FromSeconds(1), 500)
                .Select(s =>
                {
                    var r = s.Select(y => y.FullPath).Distinct();
                    return r;
                }).SelectMany(x => x);

            Deleted = Observable.FromEvent<FileSystemEventHandler, FileSystemEventArgs>(handler =>
            {
                FileSystemEventHandler fsHandler = (sender, e) =>
                {
                    handler(e);
                };

                return fsHandler;
            },
                fsHandler => _watcher.Deleted += fsHandler,
                fsHandler => _watcher.Deleted -= fsHandler)
                .Select(x => x.FullPath);
        }
    }
}