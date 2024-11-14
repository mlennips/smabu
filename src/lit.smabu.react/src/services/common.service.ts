import axiosConfig from "../configs/axiosConfig";
import { Currency, PaymentMethod, TaxRate, Unit } from "../types/domain";

export const getCurrencies = async (): Promise<Currency[]> => {
    const response = await axiosConfig.get<Currency[]>(`common/currencies`);
    return response?.data;
};

export const getUnits = async (): Promise<Unit[]> => {
    const response = await axiosConfig.get<Unit[]>(`common/units`);
    return response?.data;
};

export const getTaxRates = async (): Promise<TaxRate[]> => {
    const response = await  axiosConfig.get<TaxRate[]>(`common/taxrates`);
    return response?.data;
};

export const getPaymentMethods = async (): Promise<PaymentMethod[]> => {
    const response = await  axiosConfig.get<PaymentMethod[]>(`common/paymentMethods`);
    return response?.data;
};