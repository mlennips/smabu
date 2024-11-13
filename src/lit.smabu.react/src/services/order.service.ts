import axiosConfig from "../configs/axiosConfig";
import { CreateOrderCommand, GetOrderReferencesResponse, OrderDTO, UpdateOrderCommand, UpdateReferencesToOrderCommand } from "../types/domain";

export const createOrder = async (payload: CreateOrderCommand) => {
    const response = await axiosConfig.post<OrderDTO[]>(`orders`, payload);
    return response.data;
};

export const getOrders = async () => {
    const response = await axiosConfig.get<OrderDTO[]>(`orders`);
    return response.data;
};

export const getOrder = async (orderId: string) => {
    const response = await axiosConfig.get<OrderDTO>(`orders/${orderId}`);
    return response.data;
};

export const updateOrder = async (orderId: string, payload: UpdateOrderCommand) => {
    const response = await axiosConfig.put(`orders/${orderId}`, payload);
    return response.data;
};

export const deleteOrder = async (orderId: string) => {
    const response = await axiosConfig.delete(`orders/${orderId}`);
    return response.data;
};

export const getOrdersReferences = async (orderId: string) => {
    const response = await axiosConfig.get<GetOrderReferencesResponse>(`orders/${orderId}/references`);
    return response.data;
};

export const updateOrderReferences = async (orderId: string, payload: UpdateReferencesToOrderCommand) => {
    const response = await axiosConfig.put<UpdateReferencesToOrderCommand>(`orders/${orderId}/references`, payload);
    return response.data;
};