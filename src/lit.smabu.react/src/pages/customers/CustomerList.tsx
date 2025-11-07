import { useEffect, useState } from "react";
import { CustomerDTO } from "../../types/domain";
import { IconButton, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import DefaultContentContainer, { ToolbarItem } from "../../components/ContentBlocks/DefaultContentBlock";
import { Add, Edit } from "@mui/icons-material";
import { getCustomers } from "../../services/customer.service";
import { handleAsyncTask } from "../../utils/handleAsyncTask";

const CustomerList = () => {
    const [data, setData] = useState<CustomerDTO[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const toolbarItems: ToolbarItem[] = [
        {
            text: "Neu",
            route: "/customers/create",
            icon: <Add />
        }
    ];

    useEffect(() => {
         handleAsyncTask({
            task: () =>getCustomers(),
            onLoading: setLoading,
            onSuccess: setData,
            onError: setError
        });
    }, []);

    return (
        <DefaultContentContainer loading={loading} error={error} toolbarItems={toolbarItems}>
            <TableContainer component={Paper} >
                <Table size="medium">
                    <TableHead>
                        <TableRow>
                            <TableCell>#</TableCell>
                            <TableCell>Name</TableCell>
                            <TableCell>Kurzname</TableCell>
                            <TableCell>Branche</TableCell>
                            <TableCell>Ort</TableCell>
                            <TableCell></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {data.map((customer: CustomerDTO) => (
                            <TableRow key={customer.id?.value}>
                                <TableCell>{customer.number!.value}</TableCell>
                                <TableCell>{customer.name}</TableCell>
                                <TableCell>{customer.corporateDesign?.shortName}</TableCell>
                                <TableCell>{customer.industryBranch}</TableCell>
                                <TableCell>{customer.mainAddress?.city}</TableCell>
                                <TableCell>
                                    <IconButton size="small" LinkComponent="a" href={`/customers/${customer.id?.value}`}>
                                        <Edit />
                                    </IconButton>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </DefaultContentContainer>
    );
}

export default CustomerList;