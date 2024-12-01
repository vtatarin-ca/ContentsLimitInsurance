import type { AddItemModel } from "../models/AddItemModel";
import { LOCAL_API_BASE_URL } from "./api-config";
import axios from "axios";

export async function apiFetchItems() {
  const response = await axios.get(`${LOCAL_API_BASE_URL}/item`);
  return response.data;
}

export async function apiAddItem(newItem: AddItemModel) {
  const response = await axios.post(`${LOCAL_API_BASE_URL}/item`, newItem);
  return response.data;
}

export async function apiDeleteItem(id: string) {
  const response = await axios.delete(`${LOCAL_API_BASE_URL}/item/${id}`);
  return response;
}

export async function apiFetchCategories() {
  const response = await axios.get(`${LOCAL_API_BASE_URL}/category`);
  return response.data;
}
