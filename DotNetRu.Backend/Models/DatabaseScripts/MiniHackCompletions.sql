USE [TechDays2016]
GO

/****** Object:  Table [dbo].[Categories]    Script Date: 2016-09-02 11:45:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MiniHackCompletions](
	[UserId][nvarchar](128) NOT NULL,
	[HackId][nvarchar](128) NOT NULL,
	[CompletedAt] [datetimeoffset](7) NOT NULL,
)

GO

ALTER TABLE [dbo].[MiniHackCompletions] ADD  DEFAULT (sysutcdatetime()) FOR [CompletedAt]
GO

