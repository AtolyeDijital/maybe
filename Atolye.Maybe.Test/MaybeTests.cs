using AtolyeDijital;

namespace Atolye.Maybe.Test
{
    public class Human
    {
        public int Age { get; set; }
        public int Height { get; set; }
    }

    public class MaybeTests
    {
        

        private Task<Human> GetIntAsync(Human param1)
        {
            param1.Age += 10;
            return Task.FromResult(param1);
        }

        private Task<Human> GetHumanAsync()
        {
            var param1 = new Human { Age = 20, Height = 183 };
            return Task.FromResult(param1);
        }

        [Fact]
        public async Task ValueOrThrow_ShouldReturnUpdatedHuman()
        {
            // arrange
            var param1 = new Human { Age = 20, Height = 183 };

            // act
            var result = await Maybe<Human>
                .from(param1)
                .BindAsync(GetIntAsync)
                .With(x => x.Age += 10)
                .ValueOrThrow("Not valid int value");

            // assert
            Assert.Equal(40, result.Age);
            Assert.Equal(param1.Age, result.Age);
        }

        [Fact]
        public async Task OrElseCheck()
        {
            // arrange
            var param1 = new Human { Age = 20, Height = 183 };

            // act
            var result = await Maybe<Human>
                .from(param1)
                .BindAsync(GetIntAsync)
                .With(x => x.Age += 10)
                .Check(x => x.Age > 100)
                .OrElseAsync(GetHumanAsync);

            // assert
            Assert.Equal(20, result.Age);
            Assert.Equal(param1.Age, result.Age);
        }
    }
}