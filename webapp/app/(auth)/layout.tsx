import { AuthHeader } from "@/components/features/auth/shared/auth-header";

export default function AuthLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <>
      <header className="fixed top-0 w-full">
        <AuthHeader />
      </header>
      <main>{children}</main>
    </>
  );
}
