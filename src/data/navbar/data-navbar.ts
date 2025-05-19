import type { ReactNode } from "react";

interface NavMenuItem {
  name: string
  href: string
  icon?: ReactNode | null
  target: string
}

const navMenuItems: NavMenuItem[] = [
  {
    name: "Features",
    href: "/features",
    icon: null,
    target: "",
  },

  {
    name: "Solution",
    href: "/solutions",
    icon: null,
    target: "",
  },

  {
    name: "Pricing",
    href: "/pricing",
    icon: null,
    target: "",
  },

  {
    name: "Learn",
    href: "/learn",
    icon: null,
    target: "",
  },

  {
    name: "Blog",
    href: "/blog",
    icon: null,
    target: "",
  },
];

export { navMenuItems };
