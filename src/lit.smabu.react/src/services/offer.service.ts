import axiosConfig from "../configs/axiosConfig";
import { AddOfferItemCommand, CreateOfferCommand, OfferDTO, UpdateOfferCommand, UpdateOfferItemCommand } from "../types/domain";

export const createOffer = async (payload: CreateOfferCommand) => {
    const response = await axiosConfig.post<OfferDTO[]>(`offers`, payload);
    return response.data;
};

export const getOffers = async () => {
    const response = await axiosConfig.get<OfferDTO[]>(`offers`);
    return response.data;
};

export const getOffer = async (offerId: string, withItems: boolean = false) => {
    const response = await axiosConfig.get<OfferDTO>(`offers/${offerId}?withItems=${withItems}`);
    return response.data;
};

export const getOfferReport = async (offerId: string) => {
    const response = await axiosConfig.get<any>(`offers/${offerId}/report`, {
        headers: {
          'Content-Type': 'application/json',
        },
        responseType: 'blob',
      });
    return response;
};

export const updateOffer = async (offerId: string, payload: UpdateOfferCommand) => {
    const response = await axiosConfig.put(`offers/${offerId}`, payload);
    return response.data;
};

export const deleteOffer = async (offerId: string) => {
    const response = await axiosConfig.delete(`offers/${offerId}`);
    return response.data;
};

export const addOfferItem = async (offerId: string, payload: AddOfferItemCommand) => {
    const response = await axiosConfig.post(`offers/${offerId}/items`, payload);
    return response.data;
};

export const updateOfferItem = async (offerId: string, itemId: string, payload: UpdateOfferItemCommand) => {
    const response = await axiosConfig.put(`offers/${offerId}/items/${itemId}`, payload);
    return response.data;
};

export const moveOfferItemUp = async (offerId: string, itemId: string) => {
    const response = await axiosConfig.put(`offers/${offerId}/items/${itemId}/moveUp`);
    return response.data;
};

export const moveOfferItemDown = (offerId: string, itemId: string) => {
    return axiosConfig.put(`offers/${offerId}/items/${itemId}/movedown`);
};

export const deleteOfferItem = (offerId: string, itemId: string) => {
    return axiosConfig.delete(`offers/${offerId}/items/${itemId}`);
};