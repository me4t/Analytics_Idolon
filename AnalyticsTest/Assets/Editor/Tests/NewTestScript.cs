using System.Collections;
using System.Collections.Generic;
using Code.Analytics;
using Code.EventBufferService;
using Code.SaveLoadEventService;
using Code.ServerSender;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

using UnityEngine.TestTools;

public class NewTestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void NewTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}

[TestFixture]
public class AnalyticsServiceTests
{
    private ISaveLoadEventService _mockSaveLoadService;
    private IEventBuffer _mockEventBuffer;
    private IServerEventSender _mockServerEventSender;
    private AnalyticsService _analyticsService;

    [SetUp]
    public void SetUp()
    {
        _mockSaveLoadService = Substitute.For<ISaveLoadEventService>();
        _mockEventBuffer = Substitute.For<IEventBuffer>();
        _mockServerEventSender = Substitute.For<IServerEventSender>();

        _analyticsService = new AnalyticsService(_mockSaveLoadService, _mockEventBuffer, _mockServerEventSender);
    }
}

