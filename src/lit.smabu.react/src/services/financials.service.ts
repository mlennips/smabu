import axiosConfig from "../configs/axiosConfig";
import { AnnualFinancialStatementId, CreateAnnualFinancialStatementCommand, UpdateAnnualFinancialStatementCommand } from "../types/domain";
import { AnnualFinancialStatementDTO } from '../types/domain/annual-financial-statement-dto';

export const createAnnualFinancialStatement = async (payload: CreateAnnualFinancialStatementCommand): Promise<AnnualFinancialStatementId> => {
    const response = await axiosConfig.post<AnnualFinancialStatementId>(`/financial/annualstatements/`, payload);
    return response.data;
  };

export const getAnnualFinancialStatements = async (): Promise<AnnualFinancialStatementDTO[]> => {
    const response = await axiosConfig.get('/financial/annualstatements/');
    return response.data;
};

export const getAnnualFinancialStatement = async (id: string): Promise<AnnualFinancialStatementDTO> => {
    const response = await axiosConfig.get(`/financial/annualstatements/${id}`);
    return response.data;
};

export const updateAnnualFinancialStatement = async (id: string, statement: UpdateAnnualFinancialStatementCommand): Promise<void> => {
    await axiosConfig.put(`/financial/annualstatements/${id}`, statement);
};

export const importAnnualFinancialStatementTransactions = async (id: string): Promise<void> => {
    await axiosConfig.put(`/financial/annualstatements/${id}/importtransactions`);
};

export const completeAnnualFinancialStatement = async (id: string): Promise<void> => {
    await axiosConfig.put(`/financial/annualstatements/${id}/complete`);
};