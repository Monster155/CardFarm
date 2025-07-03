using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dajjsand.Models.Task
{
    [Serializable]
    public class Tasks
    {
        [SerializeReference]
        public List<ITask> _tasks = new();
    }
}