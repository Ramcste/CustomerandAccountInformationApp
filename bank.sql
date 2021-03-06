USE [Bank]
GO
/****** Object:  Table [dbo].[customer]    Script Date: 5/16/2015 12:01:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[customer](
	[cu_id] [int] IDENTITY(1,1) NOT NULL,
	[cu_name] [varchar](50) NOT NULL,
	[cu_email] [varchar](50) NOT NULL,
	[cu_accountno] [varchar](50) NOT NULL,
	[cu_openingdate] [varchar](50) NOT NULL,
	[balance] [float] NULL,
 CONSTRAINT [PK_customer] PRIMARY KEY CLUSTERED 
(
	[cu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
