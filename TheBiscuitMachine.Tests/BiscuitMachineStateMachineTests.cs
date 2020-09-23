using System;
using System.Collections.Generic;
using System.Text;
using TheBiscuitMachine.Logic.Models;
using Xunit;

namespace TheBiscuitMachine.Tests
{
    public class BiscuitMachineStateMachineTests
    {
        [Fact]
        public void ShouldMoveFromInitialToOnState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial;

            //Act
            state = state.TurnedOn();

            //Assert
            Assert.True(state.IsOn);
        }

        [Fact]
        public void ShouldNotMoveFromInitialToPausedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial;

            //Act and assert
            Assert.Throws<InvalidOperationException>(() => state.Pause());
        }

        [Fact]
        public void ShouldNotMoveFromInitialToOffState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial;

            //Act and assert
            Assert.Throws<InvalidOperationException>(() => state.TurnedOff());
        }

        [Fact]
        public void ShouldNotMoveFromInitialToResumedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial;

            //Act and assert
            Assert.Throws<InvalidOperationException>(() => state.Resume());
        }

        [Fact]
        public void ShouldNotMoveFromInitialToOvenHeatedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial;

            //Act and assert
            Assert.Throws<InvalidOperationException>(() => state.HeatedOven());
        }

        [Fact]
        public void ShouldNotMoveFromInitialToProductionStartedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial;

            //Act and assert
            Assert.Throws<InvalidOperationException>(() => state.StartedProduction());
        }

        [Fact]
        public void ShouldMoveFromOnToOvenHeatedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn();

            //Act
            state = state.HeatedOven();

            //Assert
            Assert.True(state.IsOvenHeated);
        }

        [Fact]
        public void ShouldMoveFromOnToPausedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn();

            //Act
            state = state.Pause();

            //Assert
            Assert.True(state.IsPaused);
        }

        [Fact]
        public void ShouldNotMoveFromOnToOnState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn();

            //Act and assert
            Assert.Throws<InvalidOperationException>(() => state.TurnedOn());
        }

        [Fact]
        public void ShouldNotMoveFromOnToProductionStartedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn();

            //Act and assert
            Assert.Throws<InvalidOperationException>(() => state.StartedProduction());
        }

        [Fact]
        public void ShouldNotMoveFromOnToProductionFinishedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn();

            //Act and assert
            Assert.Throws<InvalidOperationException>(() => state.FinishedProduction());
        }

        [Fact]
        public void ShouldMoveFromPausedToOnState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn()
                .Pause();

            //Act
            state = state.TurnedOn();

            //Assert
            Assert.True(state.IsOn);
            Assert.True(!state.IsPaused);
        }

        [Fact]
        public void ShouldMoveFromPausedToOffState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn()
                .Pause();

            //Act
            state = state.TurnedOff();

            //Assert
            Assert.True(!state.IsOn);
            Assert.True(!state.IsPaused);
        }

        [Fact]
        public void ShouldMoveFromPausedToOvenHeatedPausedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn()
                .Pause();

            //Act
            state = state.HeatedOven();

            //Assert
            Assert.True(state.IsOvenHeated);
            Assert.True(state.IsPaused);
        }

        [Fact]
        public void ShouldNotMoveFromPausedToPausedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn()
                .Pause();

            //Act and assert
            Assert.Throws<InvalidOperationException>(() => state.Pause());
        }

        [Fact]
        public void ShouldNotMoveFromPausedToProductionStartedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn()
                .Pause();

            //Act and assert
            Assert.Throws<InvalidOperationException>(() => state.StartedProduction());
        }

        [Fact]
        public void ShouldNotMoveFromPausedToProductionFinishedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn()
                .Pause();

            //Act and assert
            Assert.Throws<InvalidOperationException>(() => state.FinishedProduction());
        }

        [Fact]
        public void ShouldMoveFromOvenHeatedToOvenHeatedPausedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn()
                .HeatedOven();

            //Act
            state = state.Pause();

            //Assert
            Assert.True(state.IsOvenHeated);
            Assert.True(state.IsPaused);
        }

        [Fact]
        public void ShouldMoveFromOvenHeatedToProductionStartedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn()
                .HeatedOven();

            //Act
            state = state.StartedProduction();

            //Assert
            Assert.True(state.IsOvenHeated);
            Assert.True(state.IsProductionStarted);
        }

        [Fact]
        public void ShouldMoveFromOvenHeatedToOffState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn()
                .HeatedOven();

            //Act
            state = state.TurnedOff();

            //Assert
            Assert.True(!state.IsOn);
        }

        [Fact]
        public void ShouldNotMoveFromOvenHeatedToOnState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn()
                .HeatedOven();

            //Act and assert
            Assert.Throws<InvalidOperationException>(() => state.TurnedOn());
        }

        [Fact]
        public void ShouldNotMoveFromOvenHeatedToOvenHeatedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn()
                .HeatedOven();

            //Act and assert
            Assert.Throws<InvalidOperationException>(() => state.HeatedOven());
        }

        [Fact]
        public void ShouldNotMoveFromOvenHeatedToProductionFinishedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn()
                .HeatedOven();

            //Act and assert
            Assert.Throws<InvalidOperationException>(() => state.FinishedProduction());
        }

        [Fact]
        public void ShouldMoveFromProductionStartedToProductionStartedPausedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn()
                .HeatedOven()
                .StartedProduction();

            //Act
            state = state.Pause();

            //Assert
            Assert.True(state.IsOn);
            Assert.True(state.IsOvenHeated);
            Assert.True(state.IsProductionStarted);
            Assert.True(state.IsPaused);
        }

        [Fact]
        public void ShouldMoveFromProductionStartedToFinishingProductionState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn()
                .HeatedOven()
                .StartedProduction();

            //Act
            state = state.TurnedOff();

            //Assert
            Assert.True(!state.IsOn);
            Assert.True(state.IsOvenHeated);
            Assert.True(state.IsProductionStarted);
        }

        [Fact]
        public void ShouldNotMoveFromProductionStartedToProductionFinishedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn()
                .HeatedOven()
                .StartedProduction();

            //Act and assert
            Assert.Throws<InvalidOperationException>(() => state.FinishedProduction());
        }

        [Fact]
        public void ShouldMoveFromFinishingProductionToProductionFinishedState()
        {
            //Arrange
            var state = BiscuitMachineState.Initial
                .TurnedOn()
                .HeatedOven()
                .StartedProduction()
                .TurnedOff();

            //Act
            state = state.FinishedProduction();

            //Assert
            Assert.True(!state.IsOn);
            Assert.True(state.IsOvenHeated);
            Assert.True(state.IsProductionStarted);
            Assert.True(state.IsProductionFinished);
        }
    }
}
