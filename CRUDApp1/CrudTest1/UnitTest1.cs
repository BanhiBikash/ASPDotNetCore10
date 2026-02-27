namespace CrudTest1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            int x = 10, y = 7, result, expected = 17;

            //Act
            MathTest mt = new MathTest();
            result = mt.Add(x, y);

            //Assert
            Assert.Equal(result, expected);
        }
    }
}
