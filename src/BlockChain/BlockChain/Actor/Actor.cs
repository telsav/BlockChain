using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.CSharp;

namespace BlockChain.Actor
{
    public abstract class Actor
    {
        private readonly ActionBlock<Message> _action;

        public Actor()
        {
            _action = new ActionBlock<Message>(message =>
            {
                dynamic self = this;
                dynamic mess = message;
                self.Handle(mess);
            });
        }

        public void Send(Message message)
        {
            _action.Post(message);
        }

        public Task Completion
        {
            get
            {
                _action.Complete();
                return Task.CompletedTask;
            }
        }
    }
}
