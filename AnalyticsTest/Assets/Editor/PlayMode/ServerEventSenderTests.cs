using System.Collections;
using System.Collections.Generic;
using Code;
using Code.CoroutineRunner;
using Code.Data;
using Code.EventBufferService;
using Code.ServerSender;
using Code.ServerSender.WebRequestSender;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

namespace Editor.Tests.PlayMode
{
    [TestFixture]
    public class ServerEventSenderTests
    {
        private IEventBuffer _eventBuffer;
        private IWebRequestSender _webRequestSender;
        private ICoroutineRunner _coroutineRunner;
        private ServerEventSender _serverEventSender;

        [SetUp]
        public void SetUp()
        {
            _eventBuffer = Substitute.For<IEventBuffer>();
            _webRequestSender = Substitute.For<IWebRequestSender>();
            _coroutineRunner = new GameObject("").AddComponent<UserExample>();
            _serverEventSender = new ServerEventSender(_eventBuffer, _webRequestSender, _coroutineRunner);
        }
        [UnityTest]
        public IEnumerator SendBufferedEvents_WithDelay()
        {
            var eventData = new EventData("type", "data");
            _eventBuffer.Events.Returns(new List<EventData> { eventData });
            var unityWebRequest = Substitute.For<UnityWebRequest>();
            _webRequestSender.SendWebRequest(Arg.Any<string>(), Arg.Any<string>()).Returns(unityWebRequest);

            _serverEventSender.URL = "http://fakeurl.com";
            _serverEventSender.SendBufferedEvents(withDelay: true );
            yield return new WaitForSecondsRealtime(ServerEventSender.CooldownBeforeSend + 1);
            _webRequestSender.Received().SendWebRequest("http://fakeurl.com", Arg.Any<string>());
        }
    }
}
