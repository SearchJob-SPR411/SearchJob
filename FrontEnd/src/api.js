import axios from "axios";
import { env } from "./env.js";

export const api = axios.create({
  baseURL: env.api || "http://localhost:5000/api/",
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});