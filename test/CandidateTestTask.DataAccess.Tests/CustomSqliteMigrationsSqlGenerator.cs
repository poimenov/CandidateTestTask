using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace CandidateTestTask.DataAccess.Tests;

public class CustomSqliteMigrationsSqlGenerator : SqliteMigrationsSqlGenerator
{
    public CustomSqliteMigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, IRelationalAnnotationProvider migrationsAnnotations) : base(dependencies, migrationsAnnotations)
    {
    }

    protected override void CreateTableColumns(CreateTableOperation operation, IModel? model, MigrationCommandListBuilder builder)
    {
        operation.Columns.Where(x => x.ColumnType!.Equals("nvarchar(max)", StringComparison.OrdinalIgnoreCase))
                                                .Select(x => x).ToList().ForEach(x => x.ColumnType = "text");
        base.CreateTableColumns(operation, model, builder);
    }
}

