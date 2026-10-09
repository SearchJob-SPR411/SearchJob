import { useState, useEffect } from "react";
import { AppBar, Toolbar, Typography, Button, Box, Badge } from "@mui/material";
import BookmarkBorderIcon from "@mui/icons-material/BookmarkBorder";
import AddCircleIcon from "@mui/icons-material/AddCircle";
import AssignmentOutlinedIcon from "@mui/icons-material/AssignmentOutlined";
import { Link } from "react-router-dom";
import { getLocalFavorites } from "../../services/favoritesService";
import { getLocalApplications } from "../../services/applicationService";

export default function Navbar() {
    const [favCount, setFavCount] = useState(0);
    const [appCount, setAppCount] = useState(0);

    useEffect(() => {
        const updateCounts = () => {
            setFavCount(getLocalFavorites().length);
            setAppCount(getLocalApplications().length);
        };

        updateCounts();
        window.addEventListener("favorites-changed", updateCounts);
        window.addEventListener("applications-changed", updateCounts);

        return () => {
            window.removeEventListener("favorites-changed", updateCounts);
            window.removeEventListener("applications-changed", updateCounts);
        };
    }, []);

    return (
        <AppBar position="static">
            <Toolbar>
                <Typography
                    component={Link}
                    to="/"
                    variant="h6"
                    sx={{
                        flexGrow: 1,
                        color: "inherit",
                        textDecoration: "none",
                        fontWeight: "bold",
                        letterSpacing: 0.5,
                    }}
                >
                    SearchJob
                </Typography>

                <Box sx={{ display: "flex", gap: 1, alignItems: "center" }}>
                    <Button
                        component={Link}
                        to="/"
                        color="inherit"
                    >
                        Вакансії
                    </Button>

                    <Button
                        component={Link}
                        to="/employer/dashboard"
                        color="inherit"
                    >
                        Кабінет роботодавця
                    </Button>

                    <Button
                        component={Link}
                        to="/companies"
                        color="inherit"
                    >
                        Компанії
                    </Button>

                    <Button
                        component={Link}
                        to="/favorites"
                        color="inherit"
                        startIcon={
                            <Badge badgeContent={favCount} color="secondary" max={99}>
                                <BookmarkBorderIcon />
                            </Badge>
                        }
                    >
                        Обране
                    </Button>

                    <Button
                        component={Link}
                        to="/applications"
                        color="inherit"
                        startIcon={
                            <Badge badgeContent={appCount} color="primary" max={99}>
                                <AssignmentOutlinedIcon />
                            </Badge>
                        }
                    >
                        Заявки
                    </Button>

                    <Button
                        component={Link}
                        to="/vacancies/create"
                        variant="outlined"
                        color="inherit"
                        startIcon={<AddCircleIcon />}
                        sx={{ ml: 1 }}
                    >
                        Додати вакансію
                    </Button>
                </Box>
            </Toolbar>
        </AppBar>
    );
}