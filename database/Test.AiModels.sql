USE [Test];
GO
IF OBJECT_ID(N'dbo.AiModels', N'U') IS NULL
BEGIN
 CREATE TABLE dbo.AiModels
 (
  AiModelId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AiModels PRIMARY KEY,
  Name NVARCHAR(120) NOT NULL,
  Provider NVARCHAR(100) NOT NULL,
  Description NVARCHAR(500) NOT NULL,
  WebsiteUrl NVARCHAR(400) NULL,
  IsActive BIT NOT NULL CONSTRAINT DF_AiModels_IsActive DEFAULT (1),
  CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_AiModels_CreatedAt DEFAULT (SYSUTCDATETIME()),
  CONSTRAINT UQ_AiModels_Name_Provider UNIQUE (Name, Provider)
 );
END;
GO
INSERT INTO dbo.AiModels (Name, Provider, Description, WebsiteUrl)
SELECT seed.Name, seed.Provider, seed.Description, seed.WebsiteUrl
FROM (VALUES
 (N'GPT-4o', N'OpenAI', N'Multimodal model for text, vision, and audio tasks.', N'https://openai.com/'),
 (N'Claude 3.5 Sonnet', N'Anthropic', N'General purpose model for analysis and coding.', N'https://www.anthropic.com/'),
 (N'Gemini 1.5 Pro', N'Google', N'Multimodal model with long context support.', N'https://deepmind.google/technologies/gemini/')
) AS seed(Name, Provider, Description, WebsiteUrl)
WHERE NOT EXISTS (SELECT 1 FROM dbo.AiModels existing WHERE existing.Name = seed.Name AND existing.Provider = seed.Provider);
GO
