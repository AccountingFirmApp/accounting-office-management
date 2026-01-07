-- ===========================
-- AccountingSystem - PostgreSQL MVP Schema
-- מחיקה מלאה + יצירה מחדש
-- ===========================

-- 🔥 מחיקת כל הטבלאות אם קיימות (בסדר הפוך!)
DROP TABLE IF EXISTS AuditLog CASCADE;
DROP TABLE IF EXISTS Task CASCADE;
DROP TABLE IF EXISTS TaskType CASCADE;
DROP TABLE IF EXISTS ReportInstance CASCADE;
DROP TABLE IF EXISTS CompanyReportConfig CASCADE;
DROP TABLE IF EXISTS ReportType CASCADE;
DROP TABLE IF EXISTS CompanyContact CASCADE;
DROP TABLE IF EXISTS CompanyWorker CASCADE;
DROP TABLE IF EXISTS Company CASCADE;
DROP TABLE IF EXISTS Frequency CASCADE;
DROP TABLE IF EXISTS Worker CASCADE;
DROP TABLE IF EXISTS Role CASCADE;
DROP TABLE IF EXISTS AccountingFirm CASCADE;

-- מחיקת SEQUENCES (אם קיימים)
DROP SEQUENCE IF EXISTS accountingfirm_id_seq CASCADE;
DROP SEQUENCE IF EXISTS role_id_seq CASCADE;
DROP SEQUENCE IF EXISTS worker_id_seq CASCADE;
DROP SEQUENCE IF EXISTS frequency_id_seq CASCADE;
DROP SEQUENCE IF EXISTS company_id_seq CASCADE;
DROP SEQUENCE IF EXISTS companyworker_id_seq CASCADE;
DROP SEQUENCE IF EXISTS companycontact_id_seq CASCADE;
DROP SEQUENCE IF EXISTS reporttype_id_seq CASCADE;
DROP SEQUENCE IF EXISTS companyreportconfig_id_seq CASCADE;
DROP SEQUENCE IF EXISTS reportinstance_id_seq CASCADE;
DROP SEQUENCE IF EXISTS tasktype_id_seq CASCADE;
DROP SEQUENCE IF EXISTS task_id_seq CASCADE;
DROP SEQUENCE IF EXISTS auditlog_id_seq CASCADE;

-- מחיקת ENUMs (אם קיימים)
DROP TYPE IF EXISTS report_status CASCADE;
DROP TYPE IF EXISTS payment_method CASCADE;
DROP TYPE IF EXISTS task_status CASCADE;
DROP TYPE IF EXISTS task_category CASCADE;

-- ===========================
-- יצירת ENUMs
-- ===========================

CREATE TYPE report_status AS ENUM ('Pending', 'Reported', 'Paid', 'Approved', 'NotRequired');
CREATE TYPE payment_method AS ENUM ('Credit', 'Transfer', 'Check', 'Online', 'Cash');
CREATE TYPE task_status AS ENUM ('Pending', 'InProgress', 'Done', 'Paid', 'NotRequired');
CREATE TYPE task_category AS ENUM ('Banks', 'Income', 'Expenses', 'Reconciliations', 'Other');

-- ===========================
-- יצירת טבלאות
-- ===========================

-- 1. משרד הנהלת החשבונות
CREATE TABLE AccountingFirm (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Address TEXT,
    Email VARCHAR(255),
    Phone VARCHAR(50),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 2. תפקידים כלליים (Admin, Employee)
CREATE TABLE Role (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE,
    Description TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 3. עובדים (בלי Authentication!)
CREATE TABLE Worker (
    Id SERIAL PRIMARY KEY,
    FirmId INT NOT NULL,
    RoleId INT NOT NULL,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Phone VARCHAR(50),
    EmployeeId VARCHAR(50),
    IsActive BOOLEAN DEFAULT TRUE,
    HireDate DATE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_worker_firm FOREIGN KEY (FirmId) REFERENCES AccountingFirm(Id) ON DELETE CASCADE,
    CONSTRAINT fk_worker_role FOREIGN KEY (RoleId) REFERENCES Role(Id) ON DELETE RESTRICT
);

CREATE INDEX idx_worker_email ON Worker(Email);
CREATE INDEX idx_worker_firm ON Worker(FirmId);

-- 4. תדירויות דיווח
CREATE TABLE Frequency (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE,
    Description VARCHAR(255),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 5. חברות/לקוחות
CREATE TABLE Company (
    Id SERIAL PRIMARY KEY,
    FirmId INT NOT NULL,
    Name VARCHAR(255) NOT NULL,
    TaxId VARCHAR(20) UNIQUE,
    Address TEXT,
    Phone VARCHAR(50),
    Email VARCHAR(255),
    Notes TEXT,
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_company_firm FOREIGN KEY (FirmId) REFERENCES AccountingFirm(Id) ON DELETE CASCADE
);

CREATE INDEX idx_company_firm ON Company(FirmId);
CREATE INDEX idx_company_taxid ON Company(TaxId);

-- 6. קשר עובד-לקוח (פשוט!)
CREATE TABLE CompanyWorker (
    Id SERIAL PRIMARY KEY,
    CompanyId INT NOT NULL,
    WorkerId INT NOT NULL,
    IsActive BOOLEAN DEFAULT TRUE,
    AssignedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_companyworker_company FOREIGN KEY (CompanyId) REFERENCES Company(Id) ON DELETE CASCADE,
    CONSTRAINT fk_companyworker_worker FOREIGN KEY (WorkerId) REFERENCES Worker(Id) ON DELETE CASCADE,
    CONSTRAINT uq_company_worker UNIQUE (CompanyId, WorkerId)
);

CREATE INDEX idx_company_worker ON CompanyWorker(CompanyId, WorkerId);

-- 7. אנשי קשר של לקוח
CREATE TABLE CompanyContact (
    Id SERIAL PRIMARY KEY,
    CompanyId INT NOT NULL,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Phone VARCHAR(50),
    Email VARCHAR(255),
    Position VARCHAR(100),
    IsPrimary BOOLEAN DEFAULT FALSE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_contact_company FOREIGN KEY (CompanyId) REFERENCES Company(Id) ON DELETE CASCADE
);

CREATE INDEX idx_contact_company ON CompanyContact(CompanyId);

-- 8. סוגי דיווחים
CREATE TABLE ReportType (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    ShortCode VARCHAR(20),
    Description TEXT,
    OfficialUrl VARCHAR(255),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 9. הגדרת דיווחים פר-לקוח
CREATE TABLE CompanyReportConfig (
    Id SERIAL PRIMARY KEY,
    CompanyId INT NOT NULL,
    ReportTypeId INT NOT NULL,
    FrequencyId INT NOT NULL,
    DayOfMonth SMALLINT CHECK (DayOfMonth BETWEEN 1 AND 31),
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_config_company FOREIGN KEY (CompanyId) REFERENCES Company(Id) ON DELETE CASCADE,
    CONSTRAINT fk_config_reporttype FOREIGN KEY (ReportTypeId) REFERENCES ReportType(Id) ON DELETE RESTRICT,
    CONSTRAINT fk_config_frequency FOREIGN KEY (FrequencyId) REFERENCES Frequency(Id) ON DELETE RESTRICT,
    CONSTRAINT uq_company_report UNIQUE (CompanyId, ReportTypeId)
);

CREATE INDEX idx_config_company ON CompanyReportConfig(CompanyId);

-- 10. מופעי דיווחים
CREATE TABLE ReportInstance (
    Id SERIAL PRIMARY KEY,
    ConfigId INT NOT NULL,
    Period DATE NOT NULL,
    Amount DECIMAL(12,2),
    Status report_status DEFAULT 'Pending',
    PaymentMethod payment_method,
    ReceiptDate DATE,
    ReportedDate DATE,
    PaidDate DATE,
    Comments TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_instance_config FOREIGN KEY (ConfigId) REFERENCES CompanyReportConfig(Id) ON DELETE CASCADE
);

CREATE INDEX idx_report_period ON ReportInstance(Period);
CREATE INDEX idx_report_status ON ReportInstance(Status);
CREATE INDEX idx_report_config ON ReportInstance(ConfigId);

-- 11. סוגי משימות
CREATE TABLE TaskType (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Category task_category NOT NULL,
    DefaultOrder INT DEFAULT 99,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 12. משימות (פשוט!)
CREATE TABLE Task (
    Id SERIAL PRIMARY KEY,
    CompanyId INT NOT NULL,
    TaskTypeId INT NOT NULL,
    Period DATE NOT NULL,
    Status task_status DEFAULT 'Pending',
    DueDate DATE,
    CompletedDate DATE,
    AssignedWorkerId INT,
    Notes TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_task_company FOREIGN KEY (CompanyId) REFERENCES Company(Id) ON DELETE CASCADE,
    CONSTRAINT fk_task_tasktype FOREIGN KEY (TaskTypeId) REFERENCES TaskType(Id) ON DELETE RESTRICT,
    CONSTRAINT fk_task_worker FOREIGN KEY (AssignedWorkerId) REFERENCES Worker(Id) ON DELETE SET NULL,
    CONSTRAINT uq_task_period UNIQUE (CompanyId, TaskTypeId, Period)
);

CREATE INDEX idx_task_company_period ON Task(CompanyId, Period);
CREATE INDEX idx_task_status ON Task(Status);
CREATE INDEX idx_task_assigned ON Task(AssignedWorkerId);

-- 13. יומן ביקורת (גמיש!)
CREATE TABLE AuditLog (
    Id BIGSERIAL PRIMARY KEY,
    WorkerId INT NOT NULL,
    EntityType VARCHAR(50) NOT NULL,
    EntityId INT NOT NULL,
    Action VARCHAR(100) NOT NULL,
    OldValue TEXT,
    NewValue TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_audit_worker FOREIGN KEY (WorkerId) REFERENCES Worker(Id) ON DELETE CASCADE
);

CREATE INDEX idx_audit_entity ON AuditLog(EntityType, EntityId);
CREATE INDEX idx_audit_worker ON AuditLog(WorkerId);
CREATE INDEX idx_audit_created ON AuditLog(CreatedAt);

-- ===========================
-- Triggers לעדכון אוטומטי של UpdatedAt
-- ===========================

-- Function לעדכון UpdatedAt
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.UpdatedAt = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Triggers
CREATE TRIGGER update_accountingfirm_updated_at BEFORE UPDATE ON AccountingFirm
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_worker_updated_at BEFORE UPDATE ON Worker
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_company_updated_at BEFORE UPDATE ON Company
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_companycontact_updated_at BEFORE UPDATE ON CompanyContact
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_companyreportconfig_updated_at BEFORE UPDATE ON CompanyReportConfig
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_reportinstance_updated_at BEFORE UPDATE ON ReportInstance
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_task_updated_at BEFORE UPDATE ON Task
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

-- ===========================
-- Data Seed (נתונים ראשוניים)
-- ===========================

-- משרד ברירת מחדל
INSERT INTO AccountingFirm (Name, Address, Email, Phone) VALUES 
('משרד הנהלת חשבונות דוגמה', 'תל אביב, ישראל', 'office@example.com', '03-1234567');

-- Roles
INSERT INTO Role (Name, Description) VALUES 
('Admin', 'מנהל מערכת - גישה מלאה'),
('Manager', 'מנהל - גישה לניהול לקוחות ודיווחים'),
('Employee', 'עובד - גישה לביצוע משימות');

-- Frequencies
INSERT INTO Frequency (Name, Description) VALUES 
('Monthly', 'דיווח חודשי'),
('Bimonthly', 'דיווח דו-חודשי'),
('Quarterly', 'דיווח רבעוני'),
('Annual', 'דיווח שנתי');

-- Report Types
INSERT INTO ReportType (Name, ShortCode, Description, OfficialUrl) VALUES 
('מע"מ', 'VAT', 'דיווח מס ערך מוסף', 'https://taxes.gov.il'),
('ביטוח לאומי', 'NI', 'דיווח תשלומי ביטוח לאומי', 'https://www.btl.gov.il'),
('ניכוי מס במקור', 'WHT', 'דיווח ניכוי מס במקור - טופס 856/126', 'https://taxes.gov.il'),
('דוח רווח והפסד', 'PL', 'דוח רווח והפסד תקופתי', NULL),
('מאזן', 'BS', 'מאזן תקופתי', NULL);

-- Task Types
INSERT INTO TaskType (Name, Category, DefaultOrder) VALUES 
('קליטת דפי בנק', 'Banks', 1),
('קליטת הכנסות', 'Income', 2),
('קליטת הוצאות', 'Expenses', 3),
('קליטת שכר', 'Expenses', 4),
('התאמת בנק', 'Reconciliations', 5),
('התאמת קופה', 'Reconciliations', 6),
('סגירת חודש', 'Other', 7),
('הכנת דוחות', 'Other', 8);

-- עובד ראשוני (Admin)
INSERT INTO Worker (FirmId, RoleId, FirstName, LastName, Email, Phone, EmployeeId, HireDate) 
VALUES (1, 1, 'מנהל', 'ראשי', 'admin@example.com', '050-1234567', 'EMP001', CURRENT_DATE);

-- ===========================
-- Views שימושיים
-- ===========================

-- View: חברות עם פרטי משרד
CREATE OR REPLACE VIEW vw_CompanyDetails AS
SELECT 
    c.Id,
    c.Name AS CompanyName,
    c.TaxId,
    c.Email,
    c.Phone,
    c.IsActive,
    af.Name AS FirmName,
    af.Email AS FirmEmail,
    c.CreatedAt,
    c.UpdatedAt
FROM Company c
INNER JOIN AccountingFirm af ON c.FirmId = af.Id;

-- View: דיווחים קרובים (30 יום)
CREATE OR REPLACE VIEW vw_UpcomingReports AS
SELECT 
    ri.Id,
    c.Name AS CompanyName,
    rt.Name AS ReportTypeName,
    rt.ShortCode,
    ri.Period,
    ri.Status,
    ri.Amount,
    crc.DayOfMonth,
    CASE 
        WHEN ri.Period < CURRENT_DATE THEN CURRENT_DATE - ri.Period
        ELSE 0
    END AS DaysOverdue
FROM ReportInstance ri
INNER JOIN CompanyReportConfig crc ON ri.ConfigId = crc.Id
INNER JOIN Company c ON crc.CompanyId = c.Id
INNER JOIN ReportType rt ON crc.ReportTypeId = rt.Id
WHERE ri.Status IN ('Pending', 'Reported')
  AND ri.Period >= CURRENT_DATE - INTERVAL '30 days'
ORDER BY ri.Period;

-- View: משימות פעילות
CREATE OR REPLACE VIEW vw_ActiveTasks AS
SELECT 
    t.Id,
    c.Name AS CompanyName,
    tt.Name AS TaskTypeName,
    tt.Category,
    t.Period,
    t.Status,
    t.DueDate,
    w.FirstName || ' ' || w.LastName AS AssignedWorkerName,
    t.CreatedAt,
    t.UpdatedAt
FROM Task t
INNER JOIN Company c ON t.CompanyId = c.Id
INNER JOIN TaskType tt ON t.TaskTypeId = tt.Id
LEFT JOIN Worker w ON t.AssignedWorkerId = w.Id
WHERE t.Status IN ('Pending', 'InProgress')
ORDER BY t.DueDate NULLS LAST;

-- View: עובדים עם חברות
CREATE OR REPLACE VIEW vw_WorkerCompanies AS
SELECT 
    w.Id AS WorkerId,
    w.FirstName || ' ' || w.LastName AS WorkerName,
    w.Email AS WorkerEmail,
    c.Id AS CompanyId,
    c.Name AS CompanyName,
    cw.IsActive,
    cw.AssignedAt
FROM CompanyWorker cw
INNER JOIN Worker w ON cw.WorkerId = w.Id
INNER JOIN Company c ON cw.CompanyId = c.Id
ORDER BY w.LastName, w.FirstName, c.Name;

-- ===========================
-- Stored Procedures/Functions שימושיים
-- ===========================

-- יצירת מופעי דיווחים אוטומטית לחודש מסוים
CREATE OR REPLACE FUNCTION generate_report_instances(p_period DATE)
RETURNS TABLE (
    CompanyId INT,
    CompanyName VARCHAR,
    ReportTypeId INT,
    ReportTypeName VARCHAR,
    InstanceId INT
) AS $$
BEGIN
    RETURN QUERY
    INSERT INTO ReportInstance (ConfigId, Period, Status)
    SELECT 
        crc.Id,
        p_period,
        'Pending'::report_status
    FROM CompanyReportConfig crc
    INNER JOIN Company c ON crc.CompanyId = c.Id
    INNER JOIN ReportType rt ON crc.ReportTypeId = rt.Id
    WHERE crc.IsActive = TRUE
      AND c.IsActive = TRUE
      AND NOT EXISTS (
          SELECT 1 
          FROM ReportInstance ri 
          WHERE ri.ConfigId = crc.Id 
            AND ri.Period = p_period
      )
    RETURNING 
        c.Id AS CompanyId,
        c.Name AS CompanyName,
        rt.Id AS ReportTypeId,
        rt.Name AS ReportTypeName,
        ReportInstance.Id AS InstanceId;
END;
$$ LANGUAGE plpgsql;

-- יצירת משימות אוטומטית לחודש מסוים
CREATE OR REPLACE FUNCTION generate_tasks_for_period(
    p_company_id INT,
    p_period DATE
)
RETURNS INT AS $$
DECLARE
    tasks_created INT := 0;
BEGIN
    INSERT INTO Task (CompanyId, TaskTypeId, Period, Status)
    SELECT 
        p_company_id,
        tt.Id,
        p_period,
        'Pending'::task_status
    FROM TaskType tt
    WHERE NOT EXISTS (
        SELECT 1 
        FROM Task t 
        WHERE t.CompanyId = p_company_id 
          AND t.TaskTypeId = tt.Id 
          AND t.Period = p_period
    );
    
    GET DIAGNOSTICS tasks_created = ROW_COUNT;
    RETURN tasks_created;
END;
$$ LANGUAGE plpgsql;

-- ===========================
-- הרשאות (אופציונלי)
-- ===========================

-- אם רוצה ליצור משתמש ספציפי:
-- CREATE USER accounting_user WITH PASSWORD 'your_password_here';
-- GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO accounting_user;
-- GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO accounting_user;

-- ===========================
-- סיום
-- ===========================

-- בדיקה שהכל עובד
SELECT 'Database created successfully!' AS status;
SELECT 'Total tables: ' || COUNT(*) AS table_count 
FROM information_schema.tables 
WHERE table_schema = 'public' AND table_type = 'BASE TABLE';

-- הצגת טבלאות שנוצרו
SELECT table_name 
FROM information_schema.tables 
WHERE table_schema = 'public' 
  AND table_type = 'BASE TABLE'
ORDER BY table_name;