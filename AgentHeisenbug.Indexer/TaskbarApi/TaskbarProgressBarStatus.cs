﻿namespace AgentHeisenbug.Indexer.TaskbarApi {
    internal enum TaskbarProgressBarStatus {
        NoProgress = 0,
        Indeterminate = 1,
        Normal = 2,
        Error = 4,
        Paused = 8,
    }
}