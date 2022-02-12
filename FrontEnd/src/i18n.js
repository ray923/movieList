import LanguageDetector from 'i18next-browser-languagedetector';
import i18n from "i18next";
import zhCn from "./locales/zh-cn.json";
import enUs from "./locales/en-us.json";
import { initReactI18next } from "react-i18next";

i18n.use(LanguageDetector).use(initReactI18next).init({
  resources: {
    en: {
      translation: enUs,
    },
    zh: {
      translation: zhCn,
    },
  },
  fallbackLng: "en",
  debug: false,
  interpolation: {
    escapeValue: false,
  },
});