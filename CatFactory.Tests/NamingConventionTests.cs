﻿using Xunit;

namespace CatFactory.Tests
{
    public class NamingConventionTests
    {
        [Fact]
        public void TestCamelCase()
        {
            Assert.True(NamingConvention.GetCamelCase("FOO") == "foo");
            Assert.True(NamingConvention.GetCamelCase("Bar") == "bar");
            Assert.True(NamingConvention.GetCamelCase("zaz") == "zaz");
            Assert.True(NamingConvention.GetCamelCase("FooBarZaz") == "fooBarZaz");
            Assert.True(NamingConvention.GetCamelCase("foo bar zaz") == "fooBarZaz");
            Assert.True(NamingConvention.GetCamelCase("foo_bar_zaz") == "fooBarZaz");
        }

        [Fact]
        public void TestPascalCase()
        {
            Assert.True(NamingConvention.GetPascalCase("FOO") == "Foo");
            Assert.True(NamingConvention.GetPascalCase("Bar") == "Bar");
            Assert.True(NamingConvention.GetPascalCase("zaz") == "Zaz");
            Assert.True(NamingConvention.GetPascalCase("fooBarZaz") == "FooBarZaz");
            Assert.True(NamingConvention.GetPascalCase("foo.bar.zaz") == "FooBarZaz");
            Assert.True(NamingConvention.GetPascalCase("foo_bar_zaz") == "FooBarZaz");
            Assert.True(NamingConvention.GetPascalCase("foo bar zaz") == "FooBarZaz");
        }

        [Fact]
        public void TestSnakeCase()
        {
            Assert.True(NamingConvention.GetSnakeCase("schema_id") == "schema_id");
            Assert.True(NamingConvention.GetSnakeCase("schema.id") == "schema_id");
            Assert.True(NamingConvention.GetSnakeCase("schema id") == "schema_id");
            Assert.True(NamingConvention.GetSnakeCase("FooBarZaz") == "Foo_Bar_Zaz");
            Assert.True(NamingConvention.GetSnakeCase("foo bar zaz") == "foo_bar_zaz");
            Assert.True(NamingConvention.GetSnakeCase("foo_bar_zaz") == "foo_bar_zaz");
        }

        [Fact]
        public void TestKebabCase()
        {
            Assert.True(NamingConvention.GetKebabCase("FOO") == "foo");
            Assert.True(NamingConvention.GetKebabCase("Bar") == "bar");
            Assert.True(NamingConvention.GetKebabCase("zaz") == "zaz");
            Assert.True(NamingConvention.GetKebabCase("FooBarZaz") == "foo-bar-zaz");
            Assert.True(NamingConvention.GetKebabCase("foo bar zaz") == "foo-bar-zaz");
            Assert.True(NamingConvention.GetKebabCase("foo-bar-zaz") == "foo-bar-zaz");
            Assert.True(NamingConvention.GetKebabCase("foo_bar_zaz") == "foo-bar-zaz");
        }
    }
}
