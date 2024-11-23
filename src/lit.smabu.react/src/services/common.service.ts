import axiosConfig from "../configs/axiosConfig";
import { Currency, EnumsResponse, FinancialCategory, PaymentCondition, PaymentMethod, TaxRate, Unit } from "../types/domain";

export const getCurrencies = async (): Promise<Currency[]> => {
    return getEnumValues().then((enums) => {
        return enums.currencies!;
    });
};

export const getUnits = async (): Promise<Unit[]> => {
    return getEnumValues().then((enums) => {
        return enums.units!;
    });
};

export const getTaxRates = async (): Promise<TaxRate[]> => {
    return getEnumValues().then((enums) => {
        return enums.taxRates!;
    });
};

export const getPaymentMethods = async (): Promise<PaymentMethod[]> => {
    return getEnumValues().then((enums) => {
        return enums.paymentMethods!;
    });
};

export const getPaymentConditions = async (): Promise<PaymentCondition[]> => {
    return getEnumValues().then((enums) => {
        return enums.paymentConditions!;
    });
};

export const getFinancialCategories = async (type: 'incomes' | 'expenditures'): Promise<FinancialCategory[]> => {
    if (type === 'incomes') {
        return getEnumValues().then((enums) => {
            return enums.financialCategoryIncomes!;
        });
    } else {
        return getEnumValues().then((enums) => {
            return enums.financialCategoryExpenditures!;
        });
    }
};

const getEnumValues = async (): Promise<EnumsResponse> => {
    const key = 'data_enums';
    let data = getDataFromLocalStorage<EnumsResponse>(key);
    if (!data) {
        const response = await axiosConfig.get<EnumsResponse>(`common/enums`);
        data = response?.data;
        setDataToLocalStorage(key, data);
    }
    return data;
};

const getDataFromLocalStorage = <T>(key: string): T | null => {
    const data = localStorage.getItem(key);
    return data ? JSON.parse(data) : null;
};

const setDataToLocalStorage = <T>(key: string, data: T): void => {
    localStorage.setItem(key, JSON.stringify(data));
};