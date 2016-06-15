using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyWorkflow.Actions
{
    internal class BlockWorkflowAction<T> : WorkflowAction<T>
    {
        #region Public properties

        public override WorkflowActionState State
        {
            get 
            {
                if (!BlockCount.HasValue)
                {
                    return WorkflowActionState.Ready;
                }
                else if (BlockCount.Value > UnblockedCount)
                {
                    return WorkflowActionState.Blocked;
                }
                else
                {
                    return WorkflowActionState.Ended;
                }
            }
        }

        /// <summary>
        /// Function that will return dynamicaly the number of block count.
        /// </summary>
        public Func<T, int> BlockCountFunc { get; private set; }
        /// <summary>
        /// Once resolved, keep the value of block count.
        /// </summary>
        public int? BlockCount { get; private set; }
        /// <summary>
        /// How many times the unblock method has been called.
        /// </summary>
        public int UnblockedCount { get; private set; }

        #endregion

        #region Ctor

        public BlockWorkflowAction(Func<T,int> blockCount)
        {
            BlockCountFunc = blockCount;
        }

        #endregion

        #region Public methods

        public override void Reset()
        {
            UnblockedCount = 0;
        }

        #endregion

        public override void Resolve(T obj)
        {
            BlockCount = BlockCountFunc(obj);
        }

        public override void Unblock(int unblockLevel)
        {
            lock (this)
            {
                if (unblockLevel == Int32.MaxValue)
                {
                    UnblockedCount = Int32.MaxValue;
                }
                else
                {
                    UnblockedCount += unblockLevel;
                }
            }
        }
    }
}
