namespace Wave.Commerce.IntegrationTests.Base;

[CollectionDefinition(nameof(SharedTestCollection))]
public class SharedTestCollection : ICollectionFixture<CustomWebApplicationFactory>
{
}
