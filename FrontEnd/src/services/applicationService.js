import { api } from "../api";

const STORAGE_KEY = "searchjob_my_applications";

export const getLocalApplications = () => {
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    return raw ? JSON.parse(raw) : [];
  } catch (err) {
    console.error("Failed to parse applications from localStorage", err);
    return [];
  }
};

export const saveLocalApplications = (apps) => {
  try {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(apps));
    window.dispatchEvent(new Event("applications-changed"));
  } catch (err) {
    console.error("Failed to save applications to localStorage", err);
  }
};

export const applyToJob = async ({ vacancy, vacancyId, resumeId, coverLetter, userId = null }) => {
  const targetVacancyId = vacancyId || vacancy?.id;
  let serverData = null;

  try {
    const url = userId ? `applications/apply?userId=${userId}` : "applications/apply";
    const res = await api.post(url, {
      vacancyId: Number(targetVacancyId),
      resumeId: resumeId ? Number(resumeId) : null,
      coverLetter: coverLetter || "",
    });
    serverData = res.data;
  } catch (err) {
    // If backend returns error, check if it was 409 or other
    if (err.response && err.response.status === 409) {
      throw new Error("Ви вже подали заявку на цю вакансію.");
    }
  }

  // Record in local applications as well
  const current = getLocalApplications();
  const existing = current.find((a) => Number(a.vacancyId) === Number(targetVacancyId));
  if (!serverData && existing) {
    throw new Error("Ви вже подали заявку на цю вакансію.");
  }

  const appRecord = serverData || {
    id: Date.now(),
    vacancyId: Number(targetVacancyId),
    vacancyTitle: vacancy?.title || `Vacancy #${targetVacancyId}`,
    companyName: vacancy?.companyName || "Company",
    location: vacancy?.location || "",
    coverLetter: coverLetter || "",
    status: "Pending",
    appliedAt: new Date().toISOString(),
  };

  const updated = [appRecord, ...current.filter((a) => Number(a.vacancyId) !== Number(targetVacancyId))];
  saveLocalApplications(updated);

  return appRecord;
};

export const getMyApplications = async (userId = null) => {
  try {
    const url = userId ? `applications/my?userId=${userId}` : "applications/my";
    const res = await api.get(url);
    if (Array.isArray(res.data) && res.data.length > 0) {
      saveLocalApplications(res.data);
      return res.data;
    }
  } catch {
    // fallback to local
  }
  return getLocalApplications();
};

export const getVacancyApplications = async (vacancyId, recruiterUserId = null) => {
  try {
    const url = recruiterUserId
      ? `applications/vacancy/${vacancyId}?recruiterUserId=${recruiterUserId}`
      : `applications/vacancy/${vacancyId}`;
    const res = await api.get(url);
    return res.data;
  } catch (err) {
    console.error("Failed to fetch vacancy applications", err);
    // Return empty list or local matches for demo
    const local = getLocalApplications();
    return local.filter((a) => Number(a.vacancyId) === Number(vacancyId));
  }
};

export const updateApplicationStatus = async (applicationId, status, recruiterUserId = null) => {
  try {
    const url = recruiterUserId
      ? `applications/${applicationId}/status?recruiterUserId=${recruiterUserId}`
      : `applications/${applicationId}/status`;
    await api.put(url, { status });
  } catch (err) {
    console.error("Failed to update status on server", err);
  }

  // Update in local store
  const current = getLocalApplications();
  const updated = current.map((a) => (Number(a.id) === Number(applicationId) ? { ...a, status } : a));
  saveLocalApplications(updated);
};

export const hasApplied = (vacancyId) => {
  const current = getLocalApplications();
  return current.find((a) => Number(a.vacancyId) === Number(vacancyId));
};
