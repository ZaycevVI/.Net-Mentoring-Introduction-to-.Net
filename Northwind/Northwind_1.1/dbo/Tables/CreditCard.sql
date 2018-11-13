CREATE TABLE [dbo].[CreditCard]
(
	[Id]             INT           NOT NULL,
	[ExpirationDate] DATE          NULL,
	[CardHolder]     NVARCHAR (50) NULL,
	[EmployeeID]     INT           NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_CreditCard_ToEmployees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID])
)
