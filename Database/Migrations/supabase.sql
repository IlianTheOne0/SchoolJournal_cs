-- Table order and constraints may not be valid for execution.

CREATE TABLE public.Attending (
  Id bigint GENERATED ALWAYS AS IDENTITY NOT NULL,
  Date date NOT NULL DEFAULT now(),
  UserId bigint NOT NULL,
  SubjectId bigint NOT NULL,
  Sickness boolean NOT NULL,
  CONSTRAINT Attending_pkey PRIMARY KEY (Id),
  CONSTRAINT Attending_UserId_fkey FOREIGN KEY (UserId) REFERENCES public.Users(Id),
  CONSTRAINT Attending_SubjectId_fkey FOREIGN KEY (SubjectId) REFERENCES public.Subjects(Id)
);
CREATE TABLE public.Classes (
  Id bigint GENERATED ALWAYS AS IDENTITY NOT NULL UNIQUE,
  Name text NOT NULL CHECK (length("Name") <> 0),
  Year bigint NOT NULL CHECK ("Year" > 0),
  EducationalInstitutionId bigint NOT NULL,
  CONSTRAINT Classes_pkey PRIMARY KEY (Id),
  CONSTRAINT Classes_EducationalInstitutionId_fkey FOREIGN KEY (EducationalInstitutionId) REFERENCES public.EducationalInstitutions(Id)
);
CREATE TABLE public.EducationalInstitutions (
  Id bigint GENERATED ALWAYS AS IDENTITY NOT NULL,
  Name text NOT NULL,
  CONSTRAINT EducationalInstitutions_pkey PRIMARY KEY (Id)
);
CREATE TABLE public.Enrollments (
  Id bigint GENERATED ALWAYS AS IDENTITY NOT NULL,
  UserId bigint NOT NULL UNIQUE,
  ClassId bigint NOT NULL,
  CONSTRAINT Enrollments_pkey PRIMARY KEY (Id),
  CONSTRAINT Enrollments_ClassId_fkey FOREIGN KEY (ClassId) REFERENCES public.Classes(Id)
);
CREATE TABLE public.Grades (
  Id bigint GENERATED ALWAYS AS IDENTITY NOT NULL,
  Grade bigint NOT NULL,
  Description text,
  Date date NOT NULL DEFAULT now(),
  UserId bigint NOT NULL,
  SubjectId bigint NOT NULL,
  CONSTRAINT Grades_pkey PRIMARY KEY (Id),
  CONSTRAINT Grades_UserId_fkey FOREIGN KEY (UserId) REFERENCES public.Users(Id),
  CONSTRAINT Grades_SubjectId_fkey FOREIGN KEY (SubjectId) REFERENCES public.Subjects(Id)
);
CREATE TABLE public.Statuses (
  Id bigint GENERATED ALWAYS AS IDENTITY NOT NULL,
  Status text NOT NULL UNIQUE CHECK (length("Status") <> 0),
  CONSTRAINT Statuses_pkey PRIMARY KEY (Id)
);
CREATE TABLE public.Subjects (
  Id bigint GENERATED ALWAYS AS IDENTITY NOT NULL,
  Name text NOT NULL CHECK (length("Name") <> 0),
  TeacherId bigint,
  ClassId bigint NOT NULL,
  CONSTRAINT Subjects_pkey PRIMARY KEY (Id),
  CONSTRAINT Subjects_TeacherId_fkey FOREIGN KEY (TeacherId) REFERENCES public.Users(Id),
  CONSTRAINT Subjects_ClassId_fkey FOREIGN KEY (ClassId) REFERENCES public.Classes(Id)
);
CREATE TABLE public.Users (
  Id bigint GENERATED ALWAYS AS IDENTITY NOT NULL,
  Username text NOT NULL CHECK (length("Username") <> 0),
  FullName text NOT NULL CHECK (length("FullName") <> 0),
  DateOfBirth date NOT NULL CHECK ("DateOfBirth" <= now()),
  CreatedAt date NOT NULL DEFAULT now(),
  Email text NOT NULL,
  StatusId bigint NOT NULL,
  AvatarUrl text,
  PhoneNumber text NOT NULL CHECK (length("PhoneNumber") = 10),
  EducationalInstitutionId bigint NOT NULL,
  Sex boolean NOT NULL CHECK ("Sex" = ANY (ARRAY[true, false])),
  ProfileId uuid NOT NULL,
  DateOfTheLastUpdate date NOT NULL DEFAULT now(),
  DateOfTheLastVisitToTheJournal date NOT NULL DEFAULT now(),
  CONSTRAINT Users_pkey PRIMARY KEY (Id),
  CONSTRAINT Users_StatusId_fkey FOREIGN KEY (StatusId) REFERENCES public.Statuses(Id),
  CONSTRAINT Users_EducationalInstitutionId_fkey FOREIGN KEY (EducationalInstitutionId) REFERENCES public.EducationalInstitutions(Id),
  CONSTRAINT Users_ProfileId_fkey FOREIGN KEY (ProfileId) REFERENCES auth.users(id)
);
