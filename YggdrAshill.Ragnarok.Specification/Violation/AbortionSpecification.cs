﻿using NUnit.Framework;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(Abortion))]
    internal class AbortionSpecification
    {
        [Test]
        public void ShouldExecuteActionWhenHasAborted()
        {
            var expected = false;
            var abortion = new Abortion(_ =>
            {
                expected = true;
            });

            abortion.Abort(new Exception());

            Assert.IsTrue(expected);
        }

        [Test]
        public void CannotBeGeneratedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var abortion = new Abortion(null);
            });
        }

        [Test]
        public void CannotAbortWithNull()
        {
            var abortion = new Abortion();

            Assert.Throws<ArgumentNullException>(() =>
            {
                abortion.Abort(null);
            });
        }
    }
}
