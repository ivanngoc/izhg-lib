using System;
using IziHardGames.Libs.NonEngine.Game.Abstractions;

namespace IziHardGames.Ticking.Abstractions.Lib
{
    public class TickChannelGraphBased : TickChannel
    {
        internal readonly TickGraph graph = new TickGraph();

        public virtual void Enable(int token)
        {
            throw new System.NotImplementedException();
        }
        public override int Enable(string key, Action handler)
        {
            TickNodeSingle node = new TickNodeSingle();
            node.BindHandler(handler);
            return graph.Append(node);
        }
        public override void Disable(int token)
        {
            var node = graph.EndNode;
            while (node != null)
            {
                if (node.Token == token)
                {
                    graph.Detach(node);
                    return;
                }
                node = node.Previous;
            }
            throw new NullReferenceException($"Node with token {token} Not founded");
        }
        public virtual int Disable(Action handler)
        {
            throw new System.NotImplementedException();
        }
        public override void ExecuteSync()
        {
            var start = graph.StartNode;
            var end = graph.EndNode;
            var current = start;

            while (current != end)
            {
                current.Execute();
                current = current.Next;
            }
        }
        public override void Regist(Action action)
        {
            throw new NotImplementedException();
        }
    }

    internal abstract class TickNode : IDisposable
    {
        protected static readonly Action empty = () => { };

        protected int token;
        /// <summary>
        /// Depth Level
        /// </summary>
        protected int level;
        /// <summary>
        /// Index in parent node
        /// </summary>
        protected int sibling;

        private Action? handler;
        public TickNode? Next { get; set; }
        public TickNode? Previous { get; set; }
        public int Token => token;

        public virtual void BindHandler(Action action)
        {
            this.handler = action;
        }

        internal void Execute()
        {
            handler();
        }

        internal void SetNext(TickNode node)
        {
            this.Next = node;
            node.Previous = this;
        }

        public void SetToken(int token)
        {
            this.token = token;
        }

        public virtual void Dispose()
        {
            handler = default;
            Next = default;
            Previous = default;
        }
    }

    internal sealed class TickGraph
    {
        public TickNode StartNode => start;
        public TickNode EndNode => end;

        private readonly TickNodeStart start = new TickNodeStart();
        private readonly TickNodeEnd end = new TickNodeEnd();
        private TickNode head;
        private TickNode tail;
        private int counter;

        public TickGraph()
        {
            head = start;
            tail = start;
            start.SetNext(end);
        }

        public int Append(TickNode node)
        {
            counter++;
            node.SetToken(counter);
            tail.SetNext(node);
            node.SetNext(end);
            tail = node;
            return counter;
        }

        public void Detach(TickNode node)
        {
            if (node.Next != null)
            {
                node.Previous.SetNext(node.Next);
            }
            node.Dispose();
        }
    }

    internal class TickNodeStart : TickNode
    {
        public TickNodeStart() : base()
        {
            this.BindHandler(empty);
        }
    }
    internal class TickNodeEnd : TickNode
    {
        public TickNodeEnd() : base()
        {
            this.BindHandler(empty);
        }
    }

    internal class TickNodeSingle : TickNode
    {

    }
    internal class TickNodeDemux : TickNode
    {

    }
    internal class TickNodeMux : TickNode
    {

    }
}
