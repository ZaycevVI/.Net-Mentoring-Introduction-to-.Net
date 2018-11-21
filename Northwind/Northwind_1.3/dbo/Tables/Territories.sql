﻿CREATE TABLE [dbo].[Territories] (
    [TerritoryID]          NVARCHAR (20) NOT NULL,
    [TerritoryDescription] NCHAR (50)    NOT NULL,
    [RegionID]             INT           NOT NULL,
    CONSTRAINT [PK_Territories] PRIMARY KEY NONCLUSTERED ([TerritoryID] ASC) ON [PRIMARY],
    CONSTRAINT [FK_Territories_Region] FOREIGN KEY ([RegionID]) REFERENCES [dbo].[Regions] ([RegionID])
) ON [PRIMARY];
