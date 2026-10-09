import { api } from "../api";

const STORAGE_KEY = "searchjob_favorites";

export const getLocalFavorites = () => {
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    return raw ? JSON.parse(raw) : [];
  } catch (err) {
    console.error("Failed to parse favorites from localStorage", err);
    return [];
  }
};

export const saveLocalFavorites = (favorites) => {
  try {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(favorites));
    window.dispatchEvent(new Event("favorites-changed"));
  } catch (err) {
    console.error("Failed to save favorites to localStorage", err);
  }
};

export const isLocalFavorite = (vacancyId) => {
  const list = getLocalFavorites();
  return list.some((item) => Number(item.id) === Number(vacancyId));
};

export const toggleFavorite = async (vacancy, userId = null) => {
  const currentList = getLocalFavorites();
  const exists = currentList.some((item) => Number(item.id) === Number(vacancy.id));
  let updatedList;

  if (exists) {
    updatedList = currentList.filter((item) => Number(item.id) !== Number(vacancy.id));
    saveLocalFavorites(updatedList);

    try {
      const url = userId ? `favorites/${vacancy.id}?userId=${userId}` : `favorites/${vacancy.id}`;
      await api.delete(url);
    } catch {
      // Offline or unauthenticated fallback still works via local state
    }
    return false;
  } else {
    updatedList = [vacancy, ...currentList.filter((item) => Number(item.id) !== Number(vacancy.id))];
    saveLocalFavorites(updatedList);

    try {
      const url = userId ? `favorites/${vacancy.id}?userId=${userId}` : `favorites/${vacancy.id}`;
      await api.post(url);
    } catch {
      // Offline or unauthenticated fallback still works via local state
    }
    return true;
  }
};

export const fetchFavorites = async (userId = null) => {
  try {
    const url = userId ? `favorites?userId=${userId}` : "favorites";
    const res = await api.get(url);
    if (Array.isArray(res.data) && res.data.length > 0) {
      saveLocalFavorites(res.data);
      return res.data;
    }
  } catch {
    // Return local list if API is unreachable or unauthorized
  }
  return getLocalFavorites();
};
