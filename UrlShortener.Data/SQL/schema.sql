-----For Url Shortener Persistence-------
CREATE TABLE Urls (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Code NVARCHAR(25) NOT NULL UNIQUE,
    LongUrl NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    IsActive BIT NOT NULL
);

CREATE TABLE UrlHits (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    UrlId UNIQUEIDENTIFIER NOT NULL,
    Timestamp DATETIME2 NOT NULL,
    UserAgent NVARCHAR(512) NULL,
    IpHash NVARCHAR(128) NULL,

    CONSTRAINT FK_UrlHits_Urls FOREIGN KEY (UrlId)
        REFERENCES Urls(Id)
);


------For Workflow Engine Persistence-------
CREATE TABLE WorkflowRuns (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    StartedAt DATETIME2 NOT NULL,
    CompletedAt DATETIME2 NULL,
    IsPaused BIT NOT NULL Default 0,
    IsCanceled BIT NOT NULL Default 0,
    IsCompleted BIT NOT NULL,
    PauseReason NVARCHAR(200) NULL,
    CancelReason NVARCHAR(200) NULL,
    CurrentNode NVARCHAR(200) NOT NULL,
    IsWaitingForApproval BIT NOT NULL
);

CREATE TABLE WorkflowEvents (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    WorkflowRunId UNIQUEIDENTIFIER NOT NULL,
    NodeId NVARCHAR(200) NOT NULL,
    Timestamp DATETIME2 NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    Error NVARCHAR(MAX) NULL,
    FOREIGN KEY (WorkflowRunId) REFERENCES WorkflowRuns(Id)
);


