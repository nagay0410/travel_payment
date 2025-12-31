\encoding UTF8

INSERT INTO users (
    user_name,
    email,
    password_hash,
    is_active,
    created_at,
    updated_at
)
SELECT
    'admin',
    'admin@example.com',
    'password',  -- TODO: 本番環境ではハッシュ化されたパスワードを使用すること
    true,
    CURRENT_TIMESTAMP,
    CURRENT_TIMESTAMP
WHERE NOT EXISTS (
    SELECT 1 FROM users WHERE email = 'admin@example.com'
);

