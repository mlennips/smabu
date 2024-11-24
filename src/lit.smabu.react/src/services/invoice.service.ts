import axiosConfig from "../configs/axiosConfig";
import { AddInvoiceItemCommand, CreateInvoiceCommand, InvoiceDTO, ReleaseInvoiceCommand, UpdateInvoiceCommand, UpdateInvoiceItemCommand, WithdrawReleaseInvoiceCommand } from "../types/domain";

export const createInvoice = async (payload: CreateInvoiceCommand) => {
    const response = await axiosConfig.post<InvoiceDTO[]>(`invoices`, payload);
    return response.data;
};

export const getInvoices = async () => {
    const response = await axiosConfig.get<InvoiceDTO[]>(`invoices`);
    return response.data;
};

export const getInvoice = async (invoiceId: string, withItems: boolean = false) => {
    const response = await axiosConfig.get<InvoiceDTO>(`invoices/${invoiceId}?withItems=${withItems}`);
    return response.data;
};

export const getInvoicesByCustomerId = async (customertId: string) => {
    const response = await axiosConfig.get<InvoiceDTO[]>(`invoices?customerId=${customertId}`);
    return response.data
};

export const getInvoiceReport = async (invoiceId: string) => {
    const response = await axiosConfig.get<any>(`invoices/${invoiceId}/report`, {
        headers: {
          'Content-Type': 'application/json',
        },
        responseType: 'blob',
      });
    return response;
};

export const updateInvoice = async (invoiceId: string, payload: UpdateInvoiceCommand) => {
    const response = await axiosConfig.put(`invoices/${invoiceId}`, payload);
    return response.data;
};

export const releaseInvoice = async (invoiceId: string, payload: ReleaseInvoiceCommand) => {
    const response = await axiosConfig.put<ReleaseInvoiceCommand>(`invoices/${invoiceId}/release`, payload);
    return response.data;
};

export const withdrawReleaseInvoice = async (invoiceId: string, payload: WithdrawReleaseInvoiceCommand) => {
    const response = await axiosConfig.put<WithdrawReleaseInvoiceCommand>(`invoices/${invoiceId}/withdrawrelease`, payload);
    return response.data;
};

export const deleteInvoice = async (invoiceId: string) => {
    const response = await axiosConfig.delete(`invoices/${invoiceId}`);
    return response.data;
};

export const addInvoiceItem = (invoiceId: string, payload: AddInvoiceItemCommand) => {
    return axiosConfig.post(`invoices/${invoiceId}/items`, payload);
};

export const updateInvoiceItem = (invoiceId: string, invoiceItemId: string, payload: UpdateInvoiceItemCommand) => {
    return axiosConfig.put(`invoices/${invoiceId}/items/${invoiceItemId}`, payload);
};

export const moveInvoiceItemUp = (invoiceId: string, invoiceItemId: string) => {
    return axiosConfig.put(`invoices/${invoiceId}/items/${invoiceItemId}/moveup`);
};

export const moveInvoiceItemDown = (invoiceId: string, invoiceItemId: string) => {
    return axiosConfig.put(`invoices/${invoiceId}/items/${invoiceItemId}/movedown`);
};

export const deleteInvoiceItem = (invoiceId: string, invoiceItemId: string) => {
    return axiosConfig.delete(`invoices/${invoiceId}/items/${invoiceItemId}`);
};