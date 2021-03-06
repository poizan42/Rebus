﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;
using Rebus.Tests.Contracts;
using Rebus.Workers.ThreadPoolBased;

namespace Rebus.Tests.Workers
{
    [TestFixture]
    public class TestDefaultBackoffStrategy : FixtureBase
    {
	    [Test]
	    public void WaitDoesPerformDoesPause()
	    {
			// Arrange
		    var backoffStrategy = new DefaultBackoffStrategy(new[]
		    {
			    TimeSpan.FromMilliseconds(500)
		    });

			// Act
		    var stopwatch = Stopwatch.StartNew();
		    backoffStrategy.Wait();
		    stopwatch.Stop();

			// Assert
			Assert.GreaterOrEqual(stopwatch.Elapsed, TimeSpan.FromMilliseconds(500));
	    }

	    [Test]
	    public async Task WaitAsyncDoesPause()
	    {
		    // Arrange
		    var backoffStrategy = new DefaultBackoffStrategy(new[]
		    {
			    TimeSpan.FromMilliseconds(500)
		    });

		    // Act
		    var stopwatch = Stopwatch.StartNew();
		    await backoffStrategy.WaitAsync();
		    stopwatch.Stop();

		    // Assert
		    Assert.GreaterOrEqual(stopwatch.Elapsed, TimeSpan.FromMilliseconds(500));
	    }

		[Test]
        public void BacksOffAsItShould()
        {
            var backoffStrategy = new DefaultBackoffStrategy(new[]
            {
                TimeSpan.FromMilliseconds(100), 
                TimeSpan.FromMilliseconds(500), 
                TimeSpan.FromMilliseconds(1000), 
            });

            Printt("Starting");

            var stopwatch = Stopwatch.StartNew();
            var previousElapsed = TimeSpan.Zero;

            while (stopwatch.Elapsed < TimeSpan.FromSeconds(5))
            {
                backoffStrategy.Wait();

                var waitTime = stopwatch.Elapsed - previousElapsed;
                Printt($"Waited {waitTime}");
                previousElapsed = stopwatch.Elapsed;
            }

            Printt("Done :)");
        }
    }
}