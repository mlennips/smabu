import { useEffect, useState } from "react";
import { IconButton, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import DefaultContentContainer, { ToolbarItem } from "../../components/contentBlocks/DefaultContentBlock";
import { Add, Edit } from "@mui/icons-material";
import { formatDate } from "../../utils/formatDate";
import { OfferDTO } from "../../types/domain";
import { getOffers } from "../../services/offer.service";
import { handleAsyncTask } from "../../utils/handleAsyncTask";

const OfferList = () => {
    const [data, setData] = useState<OfferDTO[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const toolbarItems: ToolbarItem[] = [
        {
            text: "Neu",
            route: "/offers/create",
            icon: <Add />
        }
    ];

    useEffect(() => {
        handleAsyncTask({
            task: getOffers,
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
                            <TableCell>Erstellt</TableCell>
                            <TableCell>Kunde</TableCell>
                            <TableCell align="right">Summe</TableCell>
                            <TableCell></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {data.map((offer: OfferDTO) => (
                            <TableRow key={offer.id?.value}>
                                <TableCell>{offer.number!.value}</TableCell>
                                <TableCell>{formatDate(offer.createdAt)}</TableCell>
                                <TableCell>{offer.customer?.name}</TableCell>
                                <TableCell align="right">{offer.amount?.toFixed(2)} {offer.currency?.isoCode}</TableCell>
                                <TableCell>
                                    <IconButton size="small" LinkComponent="a" href={`/offers/${offer.id?.value}`}>
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

export default OfferList;