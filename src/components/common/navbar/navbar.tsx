"use-client";
import { Menu, X } from "lucide-react";
import { useState } from "react";
import { Link } from "react-router";
import { Button } from "../../ui/button";
import { navMenuItems } from "../../../data/navbar/data-navbar";
import AuthLocalStorage from "../../../helper/enums/auth";
import AvatarDropdown from "../../ui/avatar-dropdown";

function Navbar() {
  const [menuState, setMenuState] = useState(false);
  const isAutenticated = Boolean(localStorage.getItem(AuthLocalStorage.status));
  const userAuthenticated = localStorage.getItem(AuthLocalStorage.user);
  const userObj = userAuthenticated ? JSON.parse(userAuthenticated) : null;

  return (
    <nav
      data-state={menuState && "active"}
      className="fixed z-20 w-full border-b bg-background/80 backdrop-blur-3xl paragraph"
    >
      <div className="px-6 mx-auto transition-all duration-300 max-w-7xl xl:px-0">
        <div className="relative flex flex-wrap items-center justify-between gap-6 py-3 lg:gap-0 lg:py-4">
          <div className="flex items-center gap-8">
            <div className="flex items-center w-full gap-12 lg:w-auto">
              <Link
                to="/"
                aria-label="home"
                className="font-sans text-2xl font-black uppercase text-foreground"
              >
                PaperNest
              </Link>
            </div>
            <div className="hidden lg:block">
              <ul className="flex gap-8 text-sm">
                {navMenuItems.map((item, index: number) => (
                  <li key={index}>
                    <Link
                      to={item.href}
                      className="block font-mono tracking-tight duration-150 text-muted-foreground hover:text-primary hover:underline"
                    >
                      <span>{item.name}</span>
                    </Link>
                  </li>
                ))}
              </ul>
            </div>
          </div>

          <button
            onClick={() => setMenuState(!menuState)}
            aria-label={menuState == true ? "Close Menu" : "Open Menu"}
            className="relative border rounded-full z-20 -m-2.5 -mr-4 block cursor-pointer p-1.5 lg:hidden"
          >
            <Menu className="in-data-[state=active]:rotate-180 in-data-[state=active]:scale-0 in-data-[state=active]:opacity-0 m-auto size-5 duration-200" />
            <X className="in-data-[state=active]:rotate-0 in-data-[state=active]:scale-100 in-data-[state=active]:opacity-100 absolute inset-0 m-auto size-5 -rotate-180 scale-0 opacity-0 duration-200" />
          </button>

          <div className="in-data-[state=active]:block lg:in-data-[state=active]:flex mb-6 hidden w-full flex-wrap items-center justify-end space-y-4 h-dvh lg:h-max shadow-zinc-300/20 md:flex-nowrap lg:m-0 lg:flex lg:w-fit lg:gap-6 lg:space-y-0 lg:border-transparent lg:bg-transparent lg:p-0 lg:shadow-none dark:shadow-none dark:lg:bg-transparent">
            <div className="lg:hidden">
              <ul className="space-y-6 text-base">
                {navMenuItems.map((item, index: number) => (
                  <li key={index}>
                    <Link
                      to={item.href}
                      className="block duration-150 text-muted-foreground hover:text-primary hover:underline"
                    >
                      <span>{item.name}</span>
                    </Link>
                  </li>
                ))}
              </ul>
            </div>

            <hr className="h-1" />

            {isAutenticated ? (
              <div className="flex flex-col items-center w-full space-y-3 font-mono tracking-tight sm:flex-row sm:gap-3 sm:space-y-0 md:w-fit text-foreground">
                <Button asChild variant="outline" size="default">
                  <Link to="https://wa.me/+6281398721671" target="_blank">
                    <span>Contact</span>
                  </Link>
                </Button>

                <Button asChild variant="outline" size="default">
                  <Link to="/auth/signin">
                    <span>Dashboard</span>
                  </Link>
                </Button>

                {userAuthenticated != null ? (
                  <AvatarDropdown name={userObj.name} email={userObj.email} />
                ) : (
                  <AvatarDropdown name="PN" email="undefined" />
                )}
              </div>
            ) : (
              <div className="flex flex-col w-full space-y-3 font-mono tracking-tight sm:flex-row sm:gap-3 sm:space-y-0 lg:w-fit text-foreground">
                <Button asChild variant="outline" size="default">
                  <Link to="https://wa.me/+6281398721671" target="_blank">
                    <span>Contact</span>
                  </Link>
                </Button>
                <Button asChild variant="outline" size="default">
                  <Link to="/signin">
                    <span>Sign in</span>
                  </Link>
                </Button>
                <Button asChild size="default">
                  <Link to="/signup">
                    <span>Create Account</span>
                  </Link>
                </Button>
              </div>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
}

export default Navbar;
