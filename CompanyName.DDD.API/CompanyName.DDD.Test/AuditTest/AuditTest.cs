using CompanyName.DDD.Domain.Common.Audit;
using CompanyName.DDD.Domain.Common.EFCore;
using CompanyName.DDD.Domain.SaleAggregate.Domain;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.DDD.Test.AuditTest
{
    [TestClass]
    public class AuditTest
    {
        private sealed class FakeCurrentUser : ICurrentUserService
        {
            public string? UserName { get; set; }
        }

        private static Context CreateContext(ICurrentUserService user, string databaseName)
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            return new Context(options, user);
        }

        [TestMethod]
        public void SaveChanges_OnInsert_SetsCreatedFieldsFromCurrentUser()
        {
            var user = new FakeCurrentUser { UserName = "alice" };
            using var context = CreateContext(user, nameof(SaveChanges_OnInsert_SetsCreatedFieldsFromCurrentUser));
            var sale = new Sale(100m, 18m, 118m, "alice");

            context.Sales.Add(sale);
            context.SaveChanges();

            Assert.AreEqual("alice", sale.CreatedBy);
            Assert.AreNotEqual(default, sale.CreatedAt);
            Assert.IsNull(sale.UpdatedBy);
            Assert.IsNull(sale.UpdatedAt);
        }

        [TestMethod]
        public void SaveChanges_OnUpdate_SetsUpdatedFieldsAndKeepsCreatedFields()
        {
            var user = new FakeCurrentUser { UserName = "alice" };
            using var context = CreateContext(user, nameof(SaveChanges_OnUpdate_SetsUpdatedFieldsAndKeepsCreatedFields));
            var sale = new Sale(100m, 18m, 118m, "alice");
            context.Sales.Add(sale);
            context.SaveChanges();
            var createdAt = sale.CreatedAt;

            user.UserName = "bob";
            context.Entry(sale).State = EntityState.Modified;
            context.SaveChanges();

            Assert.AreEqual("bob", sale.UpdatedBy);
            Assert.IsNotNull(sale.UpdatedAt);
            Assert.AreEqual("alice", sale.CreatedBy);
            Assert.AreEqual(createdAt, sale.CreatedAt);
        }

        [TestMethod]
        public void SaveChanges_OnUpdate_DoesNotOverwriteCreationDataSentByCaller()
        {
            var user = new FakeCurrentUser { UserName = "alice" };
            using var context = CreateContext(user, nameof(SaveChanges_OnUpdate_DoesNotOverwriteCreationDataSentByCaller));
            var sale = new Sale(100m, 18m, 118m, "alice");
            context.Sales.Add(sale);
            context.SaveChanges();

            // Simula un objeto que llega sin datos de creación y se marca como modificado.
            sale.CreatedBy = "mallory";
            context.Entry(sale).State = EntityState.Modified;
            context.SaveChanges();
            context.Entry(sale).Reload();

            Assert.AreEqual("alice", sale.CreatedBy);
        }

        [TestMethod]
        public void SaveChanges_WithoutUser_LeavesUserFieldsNull()
        {
            var user = new FakeCurrentUser { UserName = null };
            using var context = CreateContext(user, nameof(SaveChanges_WithoutUser_LeavesUserFieldsNull));
            var sale = new Sale(100m, 18m, 118m, "system");

            context.Sales.Add(sale);
            context.SaveChanges();

            Assert.IsNull(sale.CreatedBy);
        }
    }
}
