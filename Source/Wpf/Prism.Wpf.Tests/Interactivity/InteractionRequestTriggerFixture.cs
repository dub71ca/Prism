

using System;
using System.Windows;
using Xunit;
using Prism.Interactivity.InteractionRequest;

namespace Prism.Wpf.Tests.Interactivity
{

    public class InteractionRequestTriggerFixture
    {
        public InteractionRequest<INotification> SourceProperty { get; set; }

        [Fact]
        public void WhenSourceObjectIsSet_ShouldSubscribeToRaisedEvent()
        {
            InteractionRequest<INotification> request = new InteractionRequest<INotification>();
            TestableInteractionRequestTrigger trigger = new TestableInteractionRequestTrigger();
            DependencyObject associatedObject = new DependencyObject();

            trigger.Attach(associatedObject);
            trigger.SourceObject = request;

            Assert.True(trigger.ExecutionCount == 0);

            request.Raise(new Notification());
            Assert.True(trigger.ExecutionCount == 1);
        }

        [Fact]
        public void WhenEventIsRaised_ShouldExecuteTriggerActions()
        {
            InteractionRequest<INotification> request = new InteractionRequest<INotification>();
            TestableInteractionRequestTrigger trigger = new TestableInteractionRequestTrigger();
            DependencyObject associatedObject = new DependencyObject();
            TestableTriggerAction action = new TestableTriggerAction();

            trigger.Actions.Add(action);
            trigger.Attach(associatedObject);
            trigger.SourceObject = request;

            Assert.True(action.ExecutionCount == 0);

            request.Raise(new Notification());
            Assert.True(action.ExecutionCount == 1);
        }
    }

    public class TestableInteractionRequestTrigger : InteractionRequestTrigger
    {
        public int ExecutionCount { get; set; }

        protected override void OnEvent(EventArgs eventArgs)
        {
            this.ExecutionCount++;
            base.OnEvent(eventArgs);
        }
    }

    public class TestableTriggerAction : Microsoft.Xaml.Behaviors.TriggerAction<DependencyObject>
    {
        public int ExecutionCount { get; set; }

        protected override void Invoke(object parameter)
        {
            this.ExecutionCount++;
            if (parameter is InteractionRequestedEventArgs)
            {
                ((InteractionRequestedEventArgs)parameter).Callback();
            }
        }
    }
}
