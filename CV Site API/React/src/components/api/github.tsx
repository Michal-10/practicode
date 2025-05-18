import axios from "axios";

const API = axios.create({
  baseURL: "https://localhost:7203/api/GitHub",
});

export const getPortfolio = () => API.get("/portfolio");

export const searchRepositories = (params: {
  name?: string;
  language?: string;
  user?: string;
}) => API.get("/search", { params });
