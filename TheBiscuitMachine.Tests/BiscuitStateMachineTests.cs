using System;
using System.Collections.Generic;
using System.Text;
using TheBiscuitMachine.Logic.Common;
using TheBiscuitMachine.Logic.Models;
using Xunit;
using FluentAssertions;

namespace TheBiscuitMachine.Tests
{
    public class BiscuitStateMachineTests
    {
        [Fact]
        public void CanStampExtractedBiscuit()
        {
            //Arrange
            var biscuit = Biscuit.Extracted;

            //Act
            biscuit = biscuit.Stamp();

            //Assert
            (biscuit.State == BiscuitState.Stamped).Should().BeTrue();
        }

        [Fact]
        public void CannnotBakeExtractedBiscuit()
        {
            //Arrange
            var biscuit = Biscuit.Extracted;

            //Act
            Action act = () => biscuit.Bake();

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Biscuit is not ready for oven.");
        }

        [Fact]
        public void CannnotCollectExtractedBiscuit()
        {
            //Arrange
            var biscuit = Biscuit.Extracted;

            //Act
            Action act = () => biscuit.Collect();

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Biscuit is not fully baked.");
        }

        [Fact]
        public void CanBakeStampedBiscuit()
        {
            //Arrange
            var biscuit = Biscuit.Extracted
                .Stamp();

            //Act
            biscuit = biscuit.Bake();

            //Assert
            (biscuit.State == BiscuitState.HalfBaked).Should().BeTrue();
        }

        [Fact]
        public void CannnotStampStampedBiscuit()
        {
            //Arrange
            var biscuit = Biscuit.Extracted
                .Stamp();

            //Act
            Action act = () => biscuit.Stamp();

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Biscuit has already been stamped.");
        }

        [Fact]
        public void CannnotCollectStampedBiscuit()
        {
            //Arrange
            var biscuit = Biscuit.Extracted
                .Stamp();

            //Act
            Action act = () => biscuit.Collect();

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Biscuit is not fully baked.");
        }

        [Fact]
        public void CanBakeHalfBakedBiscuit()
        {
            //Arrange
            var biscuit = Biscuit.Extracted
                .Stamp()
                .Bake();

            //Act
            biscuit = biscuit.Bake();

            //Assert
            (biscuit.State == BiscuitState.Baked).Should().BeTrue();
        }

        [Fact]
        public void CannnotStampHalfBakedBiscuit()
        {
            //Arrange
            var biscuit = Biscuit.Extracted
                .Stamp()
                .Bake();

            //Act
            Action act = () => biscuit.Stamp();

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Biscuit has already been stamped.");
        }

        [Fact]
        public void CannnotCollectHalfBakedBiscuit()
        {
            //Arrange
            var biscuit = Biscuit.Extracted
                .Stamp()
                .Bake();

            //Act
            Action act = () => biscuit.Collect();

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Biscuit is not fully baked.");
        }

        [Fact]
        public void CanCollectBakedBiscuit()
        {
            //Arrange
            var biscuit = Biscuit.Extracted
                .Stamp()
                .Bake()
                .Bake();

            //Act
            biscuit = biscuit.Collect();

            //Assert
            (biscuit.State == BiscuitState.Collected).Should().BeTrue();
        }

        [Fact]
        public void CannnotStampBakedBiscuit()
        {
            //Arrange
            var biscuit = Biscuit.Extracted
                .Stamp()
                .Bake()
                .Bake();

            //Act
            Action act = () => biscuit.Stamp();

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Biscuit has already been stamped.");
        }

        [Fact]
        public void CannnotBakeBakedBiscuit()
        {
            //Arrange
            var biscuit = Biscuit.Extracted
                .Stamp()
                .Bake()
                .Bake();

            //Act
            Action act = () => biscuit.Bake();

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Biscuit is not ready for oven.");
        }
    }
}
