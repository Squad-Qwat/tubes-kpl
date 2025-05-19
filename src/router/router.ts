import { createBrowserRouter } from "react-router";
import MainLayout from "../layout/main-layout";
import ErrorPage from "../pages/error/_index";
import SigninPage from "../pages/landing/auth/sign-in";
import SignupPage from "../pages/landing/auth/sign-up";
import Home from "../pages/landing/home/_index";

const router = createBrowserRouter([
  {
    path: "/",
    Component: MainLayout,
    ErrorBoundary: ErrorPage,
    children: [
      {
        index: true,
        Component: Home,
      },

      {
        path: "/signin",
        Component: SigninPage,
      },
      {
        path: "/signup",
        Component: SignupPage,
      },

      // {
      //   path: "/workspace",
      //   Component: WorkspaceLayout,
      //   children: [
      //     {
      //       index: true,
      //       Component: HomeWorkspacePage
      //     }
      //   ]
      // },
    ],
  },
]);

export default router;
