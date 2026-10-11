-- Auditoría: CreatedAt / CreatedBy / UpdatedAt / UpdatedBy en las tablas de agregados.
-- Idempotente: se puede ejecutar varias veces. Ejecutar en Operations antes de desplegar.

IF COL_LENGTH('dbo.Kardex', 'CreatedAt') IS NULL
BEGIN
    ALTER TABLE dbo.Kardex ADD
        CreatedAt datetime2 NOT NULL CONSTRAINT DF_Kardex_CreatedAt DEFAULT (SYSUTCDATETIME()),
        CreatedBy nvarchar(100) NULL,
        UpdatedAt datetime2 NULL,
        UpdatedBy nvarchar(100) NULL;
END
GO

IF COL_LENGTH('dbo.Product', 'CreatedAt') IS NULL
BEGIN
    ALTER TABLE dbo.Product ADD
        CreatedAt datetime2 NOT NULL CONSTRAINT DF_Product_CreatedAt DEFAULT (SYSUTCDATETIME()),
        CreatedBy nvarchar(100) NULL,
        UpdatedAt datetime2 NULL,
        UpdatedBy nvarchar(100) NULL;
END
GO

IF COL_LENGTH('dbo.ProductStock', 'CreatedAt') IS NULL
BEGIN
    ALTER TABLE dbo.ProductStock ADD
        CreatedAt datetime2 NOT NULL CONSTRAINT DF_ProductStock_CreatedAt DEFAULT (SYSUTCDATETIME()),
        CreatedBy nvarchar(100) NULL,
        UpdatedAt datetime2 NULL,
        UpdatedBy nvarchar(100) NULL;
END
GO

IF COL_LENGTH('dbo.Sale', 'CreatedAt') IS NULL
BEGIN
    ALTER TABLE dbo.Sale ADD
        CreatedAt datetime2 NOT NULL CONSTRAINT DF_Sale_CreatedAt DEFAULT (SYSUTCDATETIME()),
        CreatedBy nvarchar(100) NULL,
        UpdatedAt datetime2 NULL,
        UpdatedBy nvarchar(100) NULL;
END
GO

IF COL_LENGTH('dbo.SaleDetail', 'CreatedAt') IS NULL
BEGIN
    ALTER TABLE dbo.SaleDetail ADD
        CreatedAt datetime2 NOT NULL CONSTRAINT DF_SaleDetail_CreatedAt DEFAULT (SYSUTCDATETIME()),
        CreatedBy nvarchar(100) NULL,
        UpdatedAt datetime2 NULL,
        UpdatedBy nvarchar(100) NULL;
END
GO

IF COL_LENGTH('dbo.TaxType', 'CreatedAt') IS NULL
BEGIN
    ALTER TABLE dbo.TaxType ADD
        CreatedAt datetime2 NOT NULL CONSTRAINT DF_TaxType_CreatedAt DEFAULT (SYSUTCDATETIME()),
        CreatedBy nvarchar(100) NULL,
        UpdatedAt datetime2 NULL,
        UpdatedBy nvarchar(100) NULL;
END
GO

