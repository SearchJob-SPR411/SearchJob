import { AppBar, Toolbar, Typography, Button, Box } from "@mui/material";
import AccountCircleOutlinedIcon from "@mui/icons-material/AccountCircleOutlined";
import { Link } from "react-router-dom";

export default function Navbar() {
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
                    }}
                >
                    SearchJob
                </Typography>

                <Box sx={{ display: "flex", gap: 1 }}>
                    <Button
                        component={Link}
                        to="/"
                        color="inherit"
                    >
                        Vacancies
                    </Button>

                    <Button
                        component={Link}
                        to="/companies"
                        color="inherit"
                    >
                        Companies
                    </Button>

                    <Button color="inherit">
                        Resumes
                    </Button>

                    <Button
                        component={Link}
                        to="/profile"
                        color="inherit"
                        startIcon={<AccountCircleOutlinedIcon />}
                    >
                        Profile
                    </Button>

                    <Button color="inherit">
                        Login
                    </Button>
                </Box>
            </Toolbar>
        </AppBar>
    );
}