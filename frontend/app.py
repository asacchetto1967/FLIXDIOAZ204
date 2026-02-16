import streamlit as st
import requests
import os

# Configurações da API (Substitua pela URL do seu APIM ou Function App)
BASE_URL = os.getenv("AZURE_FUNCTION_URL", "http://localhost:7071/api")
API_KEY = os.getenv("AZURE_FUNCTION_KEY", "[YOUR_FUNCTION_KEY]")

st.set_page_config(page_title="Netflix Catalog Manager", layout="wide")

st.title("🎬 Netflix Catalog Manager")
st.markdown("Gerencie seu catálogo de vídeos usando Azure Functions e CosmosDB.")

# --- Sidebar para Navegação ---
menu = ["Listar Filmes", "Adicionar Novo Filme", "Ver Detalhes"]
choice = st.sidebar.selectbox("Menu", menu)

if choice == "Adicionar Novo Filme":
    st.subheader("➕ Adicionar Filme ao Catálogo")
    
    with st.form("movie_form"):
        title = st.text_input("Título do Filme")
        year = st.text_input("Ano de Lançamento")
        
        video_file = st.file_uploader("Upload do Vídeo", type=["mp4", "mov", "avi"])
        thumb_file = st.file_uploader("Upload do Thumbnail", type=["jpg", "png", "jpeg"])
        
        submitted = st.form_submit_button("Salvar Filme")
        
        if submitted:
            if not title or not year:
                st.error("Título e ano são obrigatórios.")
            elif not video_file or not thumb_file:
                st.error("Vídeo e thumbnail são obrigatórios.")
            else:
                try:
                    # 1. Upload Video
                    files = {"file": (video_file.name, video_file.getvalue())}
                    v_res = requests.post(f"{BASE_URL}/video", files=files)
                    v_res.raise_for_status()
                    video_url = v_res.json().get("url")
                    
                    # 2. Upload Thumbnail
                    files = {"file": (thumb_file.name, thumb_file.getvalue())}
                    t_res = requests.post(f"{BASE_URL}/thumbnail", files=files)
                    t_res.raise_for_status()
                    thumb_url = t_res.json().get("url")
                    
                    # 3. Save Metadata to CosmosDB
                    payload = {
                        "title": title,
                        "year": year,
                        "videoUrl": video_url,
                        "thumbUrl": thumb_url
                    }
                    res = requests.post(f"{BASE_URL}/movie", json=payload)
                    res.raise_for_status()
                    
                    st.success(f"Filme '{title}' adicionado com sucesso!")
                except requests.exceptions.RequestException as e:
                    st.error(f"Erro ao processar requisição: {str(e)}")

elif choice == "Listar Filmes":
    st.subheader("📂 Catálogo Completo")
    res = requests.get(f"{BASE_URL}/movies")
    if res.status_code == 200:
        movies = res.json()
        for movie in movies:
            col1, col2 = st.columns([1, 3])
            with col1:
                st.image(movie.get("thumbUrl"), width=150)
            with col2:
                st.write(f"**{movie.get('title')}** ({movie.get('year')})")
                st.write(f"ID: {movie.get('id')}")
                if st.button(f"Ver Detalhes {movie.get('id')[:8]}", key=movie.get('id')):
                    st.session_state['selected_movie'] = movie.get('id')
                    # Aqui você poderia redirecionar ou abrir um modal
    else:
        st.error("Erro ao carregar catálogo.")

elif choice == "Ver Detalhes":
    st.subheader("🔍 Detalhes do Filme")
    movie_id = st.text_input("Insira o ID do filme")
    if st.button("Buscar"):
        res = requests.get(f"{BASE_URL}/movie/{movie_id}")
        if res.status_code == 200:
            movie = res.json()
            st.write(f"### {movie.get('title')}")
            st.image(movie.get("thumbUrl"))
            st.video(movie.get("videoUrl"))
        else:
            st.error("Filme não encontrado.")
